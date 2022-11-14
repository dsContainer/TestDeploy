using Digital.Infrastructure.Model.Requests;
using Microsoft.AspNetCore.Http;

namespace Digital.Infrastructure.Model.DocumentModel
{
    public class DocumentModels
    {
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ProcessId { get; set; }
        public Guid DocumentTypeID { get; set; }
    }

    public class DocumentCreateModel : DocumentModels
    {

    }


    public class DocumentViewModel : DocumentModels
    {
        public Guid Id { get; set; }
        public UserViewModel? Owner { get; set; }
    }

    public class DocumentUploadApiRequest
    {
        public string? FileName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ProcessId { get; set; }
        public Guid DocumentTypeId { get; set; }
    }
    public class DocumentUpdateModel
    {
        public string FileName { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
        public Guid ProcessId { get; set; }
        public Guid DocumentTypeID { get; set; }
    }

    public class DocumentResponse
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }
        public DocumentUploadApiRequest DocumentRequest { get; set; }

        public DocumentResponse()
        {
            DocumentRequest = new DocumentUploadApiRequest();
        }
    }
}
