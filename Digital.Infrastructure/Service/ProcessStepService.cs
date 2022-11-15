using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.ProcessModel;

namespace Digital.Infrastructure.Service
{
    public class ProcessStepService : IProcessStepService
    {
        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;
        public ProcessStepService(
            IMapper mapper,
            DigitalSignatureDBContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultModel> AssignProcessStep(ProcessStepCreateModel model, Guid ProcesssId)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var processStep = _mapper.Map<ProcessStep>(model);
                processStep.Id = Guid.NewGuid();
                processStep.DateCreated = DateTime.Now;
                processStep.DateUpdated = DateTime.Now;
                await _context.ProcessSteps.AddAsync(processStep);
                var process = await _context.Processes.FindAsync(ProcesssId);
                if (process != null)
                {
                    process.ProcessSteps!.Add(processStep);
                    await _context.SaveChangesAsync();

                    result.IsSuccess = true;
                    result.Code = 200;
                    result.ResponseSuccess = processStep;

                    await transaction.CommitAsync();
                }
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }

        public async Task<int> DeleteProcessStep(Guid id, bool isDeleted)
        {
            var processSteps = await _context.ProcessSteps.FindAsync(id);
            if (processSteps == null)
                throw new Exception($"Cannot find a process step with id {id}");
            processSteps.IsActive = isDeleted;
            processSteps.DateUpdated = DateTime.Now;
            _context.ProcessSteps.Update(processSteps);
            return await _context.SaveChangesAsync();
        }

        public async Task<ResultModel> GetProcessStepById(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var processStep = await _context.ProcessSteps.FindAsync(id);

                if (processStep == null)
                {
                    result.Code = 400;
                    result.IsSuccess = false;
                    result.ResponseSuccess = $"Any Process Steps Not Found!";
                    return result;
                }

                result.Code = 200;
                result.IsSuccess = true;
                result.ResponseSuccess = processStep;

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            return result;
        }

        public async Task<ResultModel> UpdateProcessStep(ProcessStepUpdateModel model)
        {
            var result = new ResultModel();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var processStep = await _context.ProcessSteps.FindAsync(model.Id);
                if (processStep == null)
                {
                    result.Code = 200;
                    result.IsSuccess = true;
                    result.ResponseSuccess = new ProcessStepUpdateModel();
                    return result;
                }

                processStep.OrderIndex = model.OrderIndex;
                processStep.UserId = model.UserId;
                processStep.Xpoint = model.Xpoint;
                processStep.Ypoint = model.Ypoint;
                processStep.XpointPercent = model.XpointPercent;
                processStep.YpointPercent = model.YpointPercent;
                processStep.Width = model.Width;
                processStep.Height = model.Height;
                processStep.PageSign = model.PageSign;
                processStep.Message = model.Message;
                processStep.DateSign = model.DateSign;
                _context.ProcessSteps.Update(processStep);
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
                result.Code = 200;
                result.IsSuccess = true;
                await transaction.CommitAsync();
                result.ResponseSuccess = processStep;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                result.IsSuccess = false;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return result;
        }

    }
}
