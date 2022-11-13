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
        public async Task<ResultModel> GetTemplate()
        {
            var result = new ResultModel();
            try
            {
                var listTemplate = await _context.Templates.ToListAsync();
                if (listTemplate.Count != 0)
                {
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = listTemplate;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Code = 400;
                    result.ResponseSuccess = "Dont have any template";
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
                        Name = model.Name,
                        NormalizationName = model.Name.ToUpperInvariant(),
                        Description = model.Description,
                        DocumentTypeId = documentTypeId,
                        IsActive = true
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
