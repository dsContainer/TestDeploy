namespace Digital.Data.Entities
{
    public partial class ProcessData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateUpload { get; set; }
        public string Url { get; set; }
        public Guid? ProcessId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }

        public virtual Process Process { get; set; }
    }
}
