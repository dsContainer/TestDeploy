using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Model
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public object? ResponseSuccess { get; set; }
        public object? ResponseFailed { get; set; }
    }

    public class APIResult<T>
    {
        public T? Result { get; set; }
    }
}
