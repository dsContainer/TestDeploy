using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.TemplateModel;

namespace Digital.Infrastructure.Interface
{
    public interface ITemplateService
    {
        Task<ResultModel> GetTemplate();
        Task<ResultModel> UploadTemplate(TemplateCreateModel model, Guid documentTypeId);
        Task<ResultModel> GetTemplateById(Guid TempalateId);
        Task<ResultModel> ChangeStatus(Guid id, bool isDeleted);
        Task<ResultModel> UpdateTemplate(Guid id, TemplateModel model);
        Task<ResultModel> DeleteTemplate(Guid id);
    }
}
