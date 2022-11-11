using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Model.DocumentModel
{
    public class DocumentModels
    {
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public Guid? DocumentTypeID { get; set; }
    }
}
