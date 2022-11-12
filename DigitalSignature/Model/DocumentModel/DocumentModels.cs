namespace DigitalSignature.Model.DocumentModel
{
    public class DocumentModels
    {
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public Guid? DocumentTypeID { get; set; }
    }
}
