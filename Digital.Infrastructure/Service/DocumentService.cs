using AutoMapper;
using Azure.Storage.Blobs;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

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
            await container.CreateAsync();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                BlobClient client = container.GetBlobClient(model.File.FileName);
                await using (Stream? data = model.File.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }
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
                var document = new Document
                {
                    Description = model!.Description,
                    FileName = model.FileName,
                    FileExtension = model.File!.FileName.Split(".").Last(),
                    DocumentTypeId = model.DocumentTypeId,
                    ProcessId = model.ProcessId,
                    OwnerId = Guid.Parse(_userContext.UserID.ToString()!),
                    Owner = _context.Users.FirstOrDefault(x => !x.IsDeleted && x.Id == Guid.Parse(ownId))
                };

                await _context.Documents.AddAsync(document);

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
    }
}
