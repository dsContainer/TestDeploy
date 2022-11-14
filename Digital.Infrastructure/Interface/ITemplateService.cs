using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.TemplateModel;

namespace Digital.Infrastructure.Interface
{
    public interface ITemplateService
    {
        Task<ResultModel> GetTemplate();
        Task<ResultModel> UploadTemplate(TemplateModel model, Guid documentTypeId);
        Task<ResultModel> GetTemplateById(Guid TempalateId);
        Task<ResultModel> ChangeStatus(string data, Guid templateId);
    }
}
