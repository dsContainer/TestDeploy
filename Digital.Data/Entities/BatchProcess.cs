namespace Digital.Data.Entities
{
    public partial class BatchProcess
    {
        public Guid BatchId { get; set; }
        public Guid ProcessId { get; set; }

        public virtual Batch Batch { get; set; }
        public virtual Process Process { get; set; }
    }
}
