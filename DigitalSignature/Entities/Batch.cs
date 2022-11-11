namespace DigitalSignature.Entities
{
    public partial class Batch
    {
        public Batch()
        {
            Processes = new HashSet<Process>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Process> Processes { get; set; }
    }
}
