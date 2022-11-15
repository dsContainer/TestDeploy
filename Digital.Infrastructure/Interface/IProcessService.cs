using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.ProcessModel;

namespace Digital.Infrastructure.Interface
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