namespace DigitalSignature.Entities
{
    public partial class Process
    {
        public Process()
        {
            Documents = new HashSet<Document>();
            ProcessData = new HashSet<ProcessData>();
            ProcessSteps = new HashSet<ProcessStep>();
            Batches = new HashSet<Batch>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? CompanyLevel { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? TemplateId { get; set; }

        public virtual Template? Template { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<ProcessData> ProcessData { get; set; }
        public virtual ICollection<ProcessStep> ProcessSteps { get; set; }

        public virtual ICollection<Batch> Batches { get; set; }
    }
}
