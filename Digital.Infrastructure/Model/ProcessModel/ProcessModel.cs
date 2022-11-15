using Digital.Data.Entities;

namespace Digital.Infrastructure.Model.ProcessModel
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

    public class ProcessViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string CompanyLevel { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? TemplateId { get; set; }

        public virtual Template? Template { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<ProcessData> ProcessData { get; set; }
        public virtual ICollection<ProcessStepModel> ProcessSteps { get; set; }
        public virtual ICollection<BatchProcess> BatchProcesses { get; set; }
    }

    public class ProcessSearchModel
    {
        public string? CreatedDate { get; set; }
    }
}
