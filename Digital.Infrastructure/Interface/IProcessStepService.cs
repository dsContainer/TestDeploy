﻿using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.ProcessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
