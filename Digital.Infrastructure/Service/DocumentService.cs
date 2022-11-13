using AutoMapper;
using Digital.Data.Data;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;
using Digital.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace Digital.Infrastructure.Service
{
    public class DocumentService : IDocumentService
    {

        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        public DocumentService(
            IHttpContextAccessor contextAccessor,
            IMapper mapper, ApplicationDBContext context,
            IUserContextService userContextService,
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _userContext = userContextService;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }
        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=dsdbstorage123;AccountKey=VM9snh83p6zT+AtAkDsFC+ivnqlJI1XAwyAQVzfFR4kX35kR3iESzXt1osAXxgSGEw49pzoM/6un+AStg2JSYQ==;EndpointSuffix=core.windows.net");


        public async Task<ResultModel> CreateAsync(DocumentUploadApiRequest model)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var fileStream = model.File!.OpenReadStream();
                //var respone = await _docManagementService.UploadAsync(model, fileStream);

                //if (respone == null)
                //{
                //    result.Code = 400;
                //    result.IsSuccess = false;
                //    result.ResponseFailed = "Create Doccument Failed!";
                //    return result;
                //}
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
                    //Id = respone!.ID,
                    //Description = respone!.Description,
                    //FileName = respone.Name,
                    FileExtension = model.File!.FileName.Split(".").Last(),
                    DocumentTypeId = model.DocumentTypeId,
                    OwnerId = Guid.Parse(_userContext.UserID.ToString()!),
                    Owner = _context.Users.FirstOrDefault(x => !x.IsDeleted && x.Id == Guid.Parse(ownId))
                };

                await _context.Documents.AddAsync(document);
                /*await _context.SaveChangesAsync();*/



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
    }
}
