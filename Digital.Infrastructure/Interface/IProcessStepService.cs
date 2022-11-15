using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.ProcessModel;

namespace Digital.Infrastructure.Interface
{
    public interface IProcessStepService
    {
        Task<ResultModel> AssignProcessStep(ProcessStepCreateModel model, Guid ProcesssId);
        Task<ResultModel> GetProcessStepById(Guid id);
        Task<int> DeleteProcessStep(Guid id, bool isDeleted);
        Task<ResultModel> UpdateProcessStep(ProcessStepUpdateModel model);
    }
}
