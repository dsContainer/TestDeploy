namespace DigitalSignature.Entities
{
    public partial class ProcessData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateUpload { get; set; }
        public Guid? ProcessId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Process Process { get; set; }
    }
}
