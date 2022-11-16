using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Model.BatchModel
{
    public class BatchModel
    {
    }
    
    public class CreateBatchModel
    {
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string description { get; set; }
        public Guid processId { get; set; }
    }
}
