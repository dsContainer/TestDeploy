using DigitalSignature.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignature.Model
{
    public class ProcessModel
    {
        public string? Name { get; set; }
        public Guid? TemplateId { get; set; }
        public string? Status { get; set; } = "Active";
        public string? CompanyLevel { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<ProcessStepModel>? ProcessSteps { get; set; } = new List<ProcessStepModel>();

    }

    public class ProcessCreateModel
    {
        public string? Name { get; set; }
        public Guid? TemplateId { get; set; }
        public string? Status { get; set; } = "Active";
        public string? CompanyLevel { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<ProcessStepCreateModel>? ProcessSteps { get; set; } = new List<ProcessStepCreateModel>();
    }
    public class ProcessUpdateModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? TemplateId { get; set; }
        public string? Status { get; set; } = "Active";
        public string? CompanyLevel { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<ProcessStepUpdateModel>? ProcessSteps { get; set; } = new List<ProcessStepUpdateModel>();
    }

    public class ProcessViewModel : ProcessModel
    {
        public Guid Id { get; set; }
        public virtual Template? Template { get; set; }
        
        public bool IsDeleted { get; set; }

        public virtual ICollection<ProcessStep>? ProcessSteps { get; set; } = new List<ProcessStep>();
    }

    public class ProcessSearchModel
    {
        public string? CreatedDate { get; set; }
    }
}
