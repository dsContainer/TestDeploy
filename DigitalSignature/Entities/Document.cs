namespace DigitalSignature.Entities
{
    public partial class Document
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string FileExtension { get; set; }
        public Guid DocumentTypeId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid ProcessId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual User Owner { get; set; }
        public virtual Process Process { get; set; }
    }
}
