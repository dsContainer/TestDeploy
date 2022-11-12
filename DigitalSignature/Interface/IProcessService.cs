using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignature.Model;

namespace DigitalSignature.Interface
{
    public interface IProcessService
    {
        Task<ResultModel> GetProcesses(ProcessSearchModel searchModel);
        Task<ResultModel> GetProcessById(Guid id);
        Task<ResultModel> CreateProcess(ProcessCreateModel model);
        Task<int> DeleteProcess(Guid id, bool isDeleted);
        Task<ResultModel> UpdateProcess(ProcessUpdateModel model);
    }
}
