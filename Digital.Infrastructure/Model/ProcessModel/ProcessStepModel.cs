namespace Digital.Infrastructure.Model.ProcessModel
{
    public class ProcessStepModel
    {
        public float? OrderIndex { get; set; }
        public Guid UserId { get; set; }
        public float Xpoint { get; set; }
        public float Ypoint { get; set; }
        public float XpointPercent { get; set; }
        public float YpointPercent { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int PageSign { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
    public class ProcessStepCreateModel : ProcessStepModel
    {

    }
    public class ProcessStepUpdateModel : ProcessStepModel
    {
        public Guid Id { get; set; }
        public DateTime DateSign { get; set; }
        public string? Message { get; set; }
    }

    public class ProcessStepViewModel : ProcessStepModel
    {
        public Guid Id { get; set; }

    }
}
