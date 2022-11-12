namespace DigitalSignature.Entities
{
    public partial class ProcessStep
    {
        public Guid Id { get; set; }
        public float? OrderIndex { get; set; }
        public Guid UserId { get; set; }
        public float Xpoint { get; set; }
        public float Ypoint { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int PageSign { get; set; }
        public DateTime DateSign { get; set; }
        public string Message { get; set; }
        public Guid? ProcessId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;
        public float XpointPercent { get; set; }
        public float YpointPercent { get; set; }

        public virtual Process Process { get; set; }
        public virtual User User { get; set; }
    }
}
