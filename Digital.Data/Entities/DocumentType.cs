namespace Digital.Data.Entities
{
    public partial class DocumentType
    {
        public DocumentType()
        {
            Documents = new HashSet<Document>();
            Templates = new HashSet<Template>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizationName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Template> Templates { get; set; }

    }
}
