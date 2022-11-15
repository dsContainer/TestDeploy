using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.TemplateModel;
using Microsoft.EntityFrameworkCore;

namespace Digital.Infrastructure.Service
{
    public class TemplateService : ITemplateService
    {
        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;
        public TemplateService(
            IMapper mapper,
            DigitalSignatureDBContext context)
        {
            _context = context;
            _mapper = mapper;
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
                            documentType = new DocumentTypeViewModel
                            {
                                id = documentTypeToPaste.Id,
                                name = documentTypeToPaste.Name,
                                normalizationName = documentTypeToPaste.NormalizationName,
                                isActive = documentTypeToPaste.IsActive,
                            },
                            dateCreated = item.DateCreated,
                            dateUpdated = item.DateUpdated,
                            isDeleted= item.IsActive
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
                        documentType = new DocumentTypeViewModel
                        {
                            id = documentType.Id,
                            name= documentType.Name,
                            normalizationName = documentType.NormalizationName,
                            isActive = documentType.IsActive,
                        },
                        dateCreated = template.DateCreated,
                        dateUpdated = template.DateUpdated,
                        isDeleted= template.IsActive
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

        public async Task<ResultModel> UploadTemplate(TemplateModel model, Guid documentTypeId)
        {
            var result = new ResultModel();
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
                    var newTemplate = new Template
                    {
                        Id = Guid.NewGuid(),
                        Name = model.name,
                        NormalizationName = model.name.ToUpperInvariant(),
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
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }
    }
}
