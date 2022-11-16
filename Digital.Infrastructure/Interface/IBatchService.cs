using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.BatchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Interface
{
    public interface IBatchService
    {
        Task<ResultModel> createBatch(CreateBatchModel model);
    }
}
