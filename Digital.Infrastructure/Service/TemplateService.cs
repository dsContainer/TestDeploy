using AutoMapper;
using Azure.Storage.Blobs;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.DocumentModel;
using Digital.Infrastructure.Model.TemplateModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Digital.Infrastructure.Service
{
    public class TemplateService : ITemplateService
    {
        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly string _storageContainerNameImage;

        public TemplateService(
            IMapper mapper,
            IConfiguration configuration,
            DigitalSignatureDBContext context)
        {
            _context = context;
            _mapper = mapper;
            _storageConnectionString = configuration["BlobConnectionString"];
            _storageContainerName = configuration["BlobContainerName"];
            _storageContainerNameImage = configuration["BlobContainerNameImage"];
        }

        public async Task<ResultModel> ChangeStatus(Guid id, bool isDeleted)
        {
            var result = new ResultModel();
            try
            {
                var templateToDo = _context.Templates.FirstOrDefault(x => x.Id == id);
                if (templateToDo == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.ResponseFailed = "Dont have any template";
                }
                else
                {
                    templateToDo.IsActive = isDeleted;
                    _context.Templates.Update(templateToDo);
                    await _context.SaveChangesAsync();
                    result.IsSuccess = true;
                    result.Code = 200;
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }


            return result;
        }

        public async Task<ResultModel> GetTemplate()
        {
            var result = new ResultModel();
            try
            {
                var listTemplate = await _context.Templates.ToListAsync();

                if (listTemplate.Count != 0)
                {
                    var listTemplateToResult = new List<TemplateViewModel>();
                    foreach (var item in listTemplate)
                    {
                        var documentTypeToPaste = _context.DocumentTypes.FirstOrDefault(x => x.Id == item.DocumentTypeId);
                        var TemplateToResult = new TemplateViewModel
                        {
                            id = item.Id,
                            name = item.Name,
                            normalizationName = item.NormalizationName,
                            description = item.Description,
                            imgUrl = item.ImgUrl,
                            ExlUrl = item.ExlUrl,
                            documentType = new Model.TemplateModel.DocumentTypeViewModel
                            {
                                id = documentTypeToPaste.Id,
                                name = documentTypeToPaste.Name,
                                normalizationName = documentTypeToPaste.NormalizationName,
                                IsActive = documentTypeToPaste.IsActive,
                            },
                            dateCreated = item.DateCreated,
                            dateUpdated = item.DateUpdated,
                            IsActive = item.IsActive
                        };
                        listTemplateToResult.Add(TemplateToResult);
                    }

                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = listTemplateToResult;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.ResponseFailed = "Dont have any template";
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }

        public async Task<ResultModel> GetTemplateById(Guid TempalateId)
        {
            var result = new ResultModel();
            try
            {
                var template = _context.Templates.FirstOrDefault(x => x.Id == TempalateId);
                if (template != null)
                {
                    var documentType = _context.DocumentTypes.FirstOrDefault(x => x.Id == template.DocumentTypeId);
                    var templateToResult = new TemplateViewModel
                    {
                        id = template.Id,
                        name = template.Name,
                        normalizationName = template.NormalizationName,
                        description = template.Description,
                        documentType = new Model.TemplateModel.DocumentTypeViewModel
                        {
                            id = documentType.Id,
                            name = documentType.Name,
                            normalizationName = documentType.NormalizationName,
                            IsActive = documentType.IsActive,
                        },
                        dateCreated = template.DateCreated,
                        dateUpdated = template.DateUpdated,
                        IsActive = template.IsActive
                    };

                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = templateToResult;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.ResponseSuccess = "Template dont found";
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }

        public async Task<ResultModel> UpdateTemplate(Guid id, TemplateModel model)
        {
            var result = new ResultModel();
            try
            {
                var templateTpUpdate = _context.Templates.FirstOrDefault(x => x.Id == id);
                if (templateTpUpdate == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.ResponseFailed = "Template not found";
                }
                else
                {
                    templateTpUpdate.Name = model.name;
                    templateTpUpdate.Description = model.description;
                    // file up storage
                    _context.Templates.Update(templateTpUpdate);
                    _context.SaveChanges();
                    result.IsSuccess = true;
                    result.Code = 200;
                }

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }

        public async Task<ResultModel> UploadTemplate(TemplateCreateModel model, Guid documentTypeId)
        {
            var result = new ResultModel();
            TemplateResponse response = new();
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            BlobContainerClient container1 = new BlobContainerClient(_storageConnectionString, _storageContainerNameImage);
            await container.CreateIfNotExistsAsync();
            BlobClient client = container.GetBlobClient(model.templateFile.FileName);
            BlobClient client1 = container1.GetBlobClient(model.jpg.FileName);

            try
            {

                var doccumentType = _context.DocumentTypes.FirstOrDefault(x => x.Id == documentTypeId);
                if (doccumentType == null)
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.ResponseSuccess = "Dont have any documentType";
                }
                else
                {
                    await using (Stream? data1 = model.jpg.OpenReadStream())
                    {
                        await client1.UploadAsync(data1);
                    }
                    await using (Stream? data = model.templateFile.OpenReadStream())
                    {   
                        await client.UploadAsync(data);
                    }
                    response.Status = $"File {model.templateFile.FileName} Uploaded Successfully";
                    response.Error = false;
                    response.Uri = client.Uri.AbsoluteUri;
                    response.UriImage = client1.Uri.AbsoluteUri;
                    response.Name = client.Name;

                    var newTemplate = new Template
                    {
                        Id = Guid.NewGuid(),
                        Name = response.Name,
                        NormalizationName = response.Name.ToUpperInvariant(),
                        ImgUrl = response.UriImage,
                        ExlUrl = response.Uri,
                        Description = model.description,
                        DocumentTypeId = documentTypeId,
                    };
                    await _context.Templates.AddAsync(newTemplate);
                    await _context.SaveChangesAsync();
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = newTemplate;
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }


        public async Task<ResultModel> DeleteTemplate(Guid id)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);
                BlobContainerClient client1 = new BlobContainerClient(_storageConnectionString, _storageContainerNameImage);
                var tmp = await _context.Templates.FirstOrDefaultAsync(x => x.Id == id);
                string filename = Path.GetFileName(new Uri(tmp.ImgUrl).AbsolutePath);
                BlobClient file = client.GetBlobClient(tmp.Name);
                BlobClient file1 = client1.GetBlobClient(filename);

                if (tmp == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseFailed = $"Doc with id: {id} not existed!!";
                    return result;
                }
                await file.DeleteAsync();
                await file1.DeleteAsync();
                _context.Templates.Remove(tmp);
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
    }
}
