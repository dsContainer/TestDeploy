using Digital.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digital.Infrastructure.Model.TemplateModel;

namespace Digital.Infrastructure.Interface
{
    public interface ITemplateService
    {
        Task<ResultModel> GetTemplate();
        Task<ResultModel> UploadTemplate(TemplateModel model, Guid documentTypeId);
    }
}
