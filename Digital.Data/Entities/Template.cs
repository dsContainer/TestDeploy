namespace Digital.Data.Entities
{
    public partial class Template
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizationName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public Guid DocumentTypeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual Process Process { get; set; }
    }
}
