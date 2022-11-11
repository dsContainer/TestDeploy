using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Data.Entities
{
    public  class Process : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        [ForeignKey("TemplateId")]
        public Guid? TemplateId { get; set; }
        public virtual Template? Template { get; set; }
        public string? Status { get; set; }
        public string? CompanyLevel { get; set; }
        [ForeignKey("ProcessStepId")]
        public string? ProcessStepId { get; set; }
        public virtual ICollection<ProcessStep>? ProcessStep { get; set; } = new List<ProcessStep>();

    }
}
