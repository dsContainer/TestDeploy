using AutoMapper;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Digital.Data.Entities;
using Digital.Data.Enums;
using Digital.Data.Utilities.Paging;
using Digital.Data.Utilities.Paging.PaginationModel;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Digital.Infrastructure.Service
{
    public class DocumentService : IDocumentService
    {

        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;

        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly ILogger<AzureBlobStorageService> _logger;

        public DocumentService(
            IMapper mapper, DigitalSignatureDBContext context,
            IUserContextService userContextService,
            IConfiguration configuration,
            ILogger<AzureBlobStorageService> logger)
        {
            _context = context;
            _mapper = mapper;
            _userContext = userContextService;
            _logger = logger;
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
            BlobClient client = container.GetBlobClient(model.File.FileName);
            try
            {

                if (response == null)
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


                await using (Stream? data = model.File.OpenReadStream())
                {
                    await client.UploadAsync(data);
                }
                response.Status = $"File {model.File.FileName} Uploaded Successfully";
                response.Error = false;
                response.Uri = client.Uri.AbsoluteUri;
                response.Name = client.Name;
                var ownId = _userContext.UserID.ToString()!;
                //add Entity
                var document = new Data.Entities.Document
                {
                    Id = Guid.NewGuid(),
                    Description = model!.Description,
                    FileName = response.Name,
                    FileExtension = response.Name.Split(".").Last(),
                    IsActive = true,
                    DocumentTypeId = model.DocumentTypeId,
                    ProcessId = model.ProcessId,
                    OwnerId = Guid.Parse(ownId),
                    Owner = _context.Users.FirstOrDefault(x => x.Id == Guid.Parse(ownId))
                };


                await _context.Documents.AddAsync(document);
                await _context.SaveChangesAsync();
                result.Code = 200;
                result.IsSuccess = true;
                result.ResponseSuccess = _mapper.Map<DocumentViewModel>(document);
            }
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"File with name {model.File.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {model.File.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                result.IsSuccess = false;
                result.ResponseFailed = "Document already exists. Please use another name to store your file";
            }
            catch (Exception e)
            {

                await transaction.RollbackAsync();
                await client.DeleteAsync();
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
                var doc = _context.Documents;

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
                BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

                var document = await _context.Documents.FirstOrDefaultAsync(x => x.Id == id);
                BlobClient file = client.GetBlobClient(document.FileName);
                if (document == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseFailed = $"Doc with id: {id} not existed!!";
                    return result;
                }
                await file.DeleteAsync();
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

                document.Description = model.Description;
                _context.Documents.Update(document);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.Code = 200;
                result.IsSuccess = true;
                await transaction.CommitAsync();
                result.ResponseSuccess = _mapper.Map<DocumentViewModel>(document);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }

        public async Task<ResultModel> GetDocumentDetail(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var doc = _context.Documents.Where(x => x.Id == id);

                if (doc == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseSuccess = $"Any Document Not Found!";
                    return result;
                }

                result.Code = 200;
                result.IsSuccess = true;
                result.ResponseSuccess = await _mapper.ProjectTo<DocumentViewModel>(doc).FirstOrDefaultAsync();

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }


        public async Task<DocumentResponse> GetContent(Guid id)
        {

            var document = await _context.Documents
            .FirstOrDefaultAsync(x => x.Id == id);
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            try
            {
                BlobClient file = client.GetBlobClient(document.FileName);

                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    var content = await file.DownloadContentAsync();

                    string name = document.FileName;
                    string contentType = content.Value.Details.ContentType;
                    return new DocumentResponse { Content = blobContent, Name = name, ContentType = contentType };
                }
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                _logger.LogError($"File {document.FileName} was not found.");
            }

            return null;
        }


        public async Task<ResultModel> GetPagingDocument(PagingParam<DocumentSortCriteria> paginationModel)
        {
            var result = new ResultModel();
            try
            {
                var documents = _context.Documents
                    .Where(x => x.OwnerId == _userContext.UserID);

                if (documents == null || documents.Count() < 0)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.ResponseFailed = $"User don't have any document";
                    return result;
                }


                var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, documents.Count());
                documents = documents.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
                documents = documents.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);

                var viewData = await _mapper.ProjectTo<DocumentViewModel>(documents).ToListAsync();

                paging.Data = viewData;

                result.IsSuccess = true;
                result.Code = 200;
                result.ResponseSuccess = paging;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }

        public async Task<ResultModel> SearchDocbyName(string textSearch)
        {
            var result = new ResultModel();
            try
            {
                var docEx = await _context.Documents.Where(x => x.FileName!.Contains(textSearch)).ToListAsync();
                if (!docEx.Any())
                {
                    result.Code = 200;
                    result.IsSuccess = true;
                    result.ResponseSuccess = new DocumentViewModel();
                    return result;
                }

                result.IsSuccess = true;
                result.Code = 200;
                result.IsSuccess = true;
                result.ResponseSuccess = _mapper.Map<List<DocumentViewModel>>(docEx);

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }
    }
}
