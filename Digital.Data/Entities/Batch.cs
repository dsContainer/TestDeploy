namespace Digital.Data.Entities
{
    public partial class Batch
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; } // send mail to teacher, add data to finish batch {Name}
        public DateTime EndDate { get; set; } 
        public Guid ProcessId { get; set; } 

    }
}
