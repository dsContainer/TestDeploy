using System.Reflection.Metadata;
using AutoMapper;
using Azure.Storage.Blobs;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;
using Digital.Infrastructure.Model.ProcessModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Document = Digital.Data.Entities.Document;

namespace Digital.Infrastructure.Service
{
    public class DocumentService : IDocumentService
    {

        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;

        public DocumentService(
            IMapper mapper, DigitalSignatureDBContext context,
            IUserContextService userContextService,
            IConfiguration configuration,
            ILogger<AzureBlobStorageService> logger)
        {
            _context = context;
            _mapper = mapper;
            _userContext = userContextService;
            _storageConnectionString = configuration["BlobConnectionString"];
            _storageContainerName = configuration["BlobContainerName"];
        }
        
        public async Task<ResultModel> CreateAsync(DocumentUploadApiRequest model)
        {
            var result = new ResultModel();
            DocumentResponse response = new();
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            await container.CreateIfNotExistsAsync();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                BlobClient client = container.GetBlobClient(model.File.FileName);
                await using (Stream? data = model.File.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }
                response.Status = $"File {model.File.FileName} Uploaded Successfully";
                response.Error = false;
                response.Uri = client.Uri.AbsoluteUri;
                response.Name = client.Name;
                if (model == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseFailed = "Create Doccument Failed!";
                    return result;
                }
                if (model.StartDate > model.EndDate)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseFailed = "Create Doccument Failed! StratDate > EndDate";
                    return result;
                }

                var ownId = _userContext.UserID.ToString()!;
                //add Entity
                var document = new Data.Entities.Document
                {
                    Id = Guid.NewGuid(),
                    Description = model!.Description,
                    FileName = model.FileName,
                    FileExtension = response.Name.Split(".").Last(),
                    DocumentTypeId = model.DocumentTypeId,
                    ProcessId = model.ProcessId,
                    OwnerId = Guid.Parse(ownId),
                    Owner = _context.Users.FirstOrDefault(x => !x.IsDeleted && x.Id == Guid.Parse(ownId))
                };

                await _context.Documents.AddAsync(document);
                await _context.SaveChangesAsync();

                result.Code = 200;
                result.IsSuccess = true;
                result.ResponseSuccess = _mapper.Map<DocumentViewModel>(document);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;

            }

            await transaction.CommitAsync();
            return result;
        }


        public async Task<ResultModel> GetDocAsync()
        {
            var result = new ResultModel();
            try
            {
                var doc= _context.Documents.Where(x => !x.IsDeleted);

                if (doc == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseSuccess = $"Any DocumentType Not Found!";
                    return result;
                }

                result.Code = 200;
                result.IsSuccess = true;
                result.ResponseSuccess = await _mapper.ProjectTo<DocumentViewModel>(doc).ToListAsync();

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }



        public async Task<ResultModel> DeleteDocument(Guid id)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

                if (document == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseFailed = $"Doc with id: {id} not existed!!";
                    return result;
                }

                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();

                result.Code = 200;
                result.ResponseSuccess = "Okie Br";
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            await transaction.CommitAsync();
            return result;
        }



        public async Task<ResultModel> UpdateDocument(DocumentUpdateModel model, Guid Id)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var document = await _context.Documents.FindAsync(Id);
                if (document == null)
                {
                    result.Code = 200;
                    result.IsSuccess = true;
                    result.ResponseSuccess = new DocumentUpdateModel();
                    return result;
                }

                document.FileName = model.FileName;
                document.Description = model.Description;
                document.DateCreated = model.DateCreate;
                document.DateUpdated = model.DateUpdate;
                _context.Documents.Update(document);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.Code = 200;
                result.IsSuccess = true;
                await transaction.CommitAsync();
                result.ResponseSuccess = document;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }
    }
}
