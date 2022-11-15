using System.ComponentModel;
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
        public TemplateService(
            IMapper mapper,
            IConfiguration configuration,
            DigitalSignatureDBContext context)
        {
            _context = context;
            _mapper = mapper;
            _storageConnectionString = configuration["BlobConnectionString"];
            _storageContainerName = configuration["BlobContainerName"];
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
                   templateToDo.IsDeleted = isDeleted;
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
                            documentType = new Model.TemplateModel.DocumentTypeViewModel
                            {
                                id = documentTypeToPaste.Id,
                                name = documentTypeToPaste.Name,
                                normalizationName = documentTypeToPaste.NormalizationName,
                                isActive = documentTypeToPaste.IsActive,
                            },
                            dateCreated = item.DateCreated,
                            dateUpdated = item.DateUpdated,
                            isDeleted= item.IsDeleted
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
                    var documentType = _context.DocumentTypes.FirstOrDefault(x=>x.Id == template.DocumentTypeId);
                    var templateToResult = new TemplateViewModel
                    {
                        id= template.Id,
                        name=template.Name,
                        normalizationName=template.NormalizationName,
                        description=template.Description,
                        documentType = new Model.TemplateModel.DocumentTypeViewModel
                        {
                            id = documentType.Id,
                            name= documentType.Name,
                            normalizationName = documentType.NormalizationName,
                            isActive = documentType.IsActive,
                        },
                        dateCreated = template.DateCreated,
                        dateUpdated = template.DateUpdated,
                        isDeleted= template.IsDeleted
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
                var templateTpUpdate = _context.Templates.FirstOrDefault(x => x.Id== id);
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
            var transaction = _context.Database.BeginTransaction();
            DocumentResponse response = new();
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            await container.CreateIfNotExistsAsync();
            BlobClient client = container.GetBlobClient(model.templateFile.FileName);
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

                    await using (Stream? data = model.templateFile.OpenReadStream())
                    {
                        await client.UploadAsync(data);
                    }
                    response.Status = $"File {model.templateFile.FileName} Uploaded Successfully";
                    response.Error = false;
                    response.Uri = client.Uri.AbsoluteUri;
                    response.Name = client.Name;
                    var newTemplate = new Template
                    {
                        Id = Guid.NewGuid(),
                        Name = response.Name,
                        NormalizationName = response.Name.ToUpperInvariant(),
                        Description = model.description,
                        DocumentTypeId = documentTypeId,
                    };
                    await _context.Templates.AddAsync(newTemplate);
                    await _context.SaveChangesAsync();
                    result.IsSuccess = true;
                    result.Code = 200;
                }
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                await client.DeleteAsync();
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }
    }
}
