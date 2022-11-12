namespace DigitalSignature.Entities
{
    public partial class Signature
    {
        public Guid Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsDelete { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
