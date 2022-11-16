using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Digital.Infrastructure.Model.TemplateModel
{
    public class TemplateModel
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string name { get; set; }
        public string? description { get; set; }
        public IFormFile? templateFile { get; set; }

    }
    public class DocumentTypeViewModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string normalizationName { get; set; }
        public bool IsActive { get; set; }

    }
    public class TemplateViewModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string normalizationName { get; set; }
        public string? description { get; set; }
        public string imgUrl { get; set; }
        public string ExlUrl { get; set; }
        public DocumentTypeViewModel? documentType { get; set; } // đưa dạng object
        public DateTime? dateCreated { get; set; }
        public DateTime? dateUpdated { get; set; }
        public bool? IsActive { get; set; }
    }


    public class TemplateCreateModel
    {
        public string? description { get; set; }
        public IFormFile? jpg { get; set; }
        public IFormFile? templateFile { get; set; }
    }


    public class TemplateResponse
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public string? UriImage { get; set; }
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? ImageName { get; set; }
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }
        public TemplateCreateModel TemplateRequest { get; set; }

        public TemplateResponse()
        {
            TemplateRequest = new TemplateCreateModel();
        }
    }
}
