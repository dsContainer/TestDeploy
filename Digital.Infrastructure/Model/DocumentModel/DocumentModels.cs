using Digital.Infrastructure.Model.Requests;
using Microsoft.AspNetCore.Http;

namespace Digital.Infrastructure.Model.DocumentModel
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

    public class DocumentCreateModel : DocumentModels
    {

    }


    public class DocumentViewModel : DocumentModels
    {
        public Guid Id { get; set; }
        public UserRequest? Owner { get; set; }
    }

    public class DocumentUploadApiRequest
    {
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public string? Description { get; set; }
        public IFormFile? File { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid DocumentTypeId { get; set; }
    }
}
