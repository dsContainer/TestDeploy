namespace Digital.Data.Entities
{
    public partial class Signature
    {
        public Guid Id { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
