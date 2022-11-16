using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Model.ProcessDataModel
{
    public class ProcessDataModel
    {

    }
    public class UploadProcessDataModel
    {
        public string processId { get; set; }
        public string processName { get; set; }
        public IFormFile templateFile { get; set; }

    }
}
