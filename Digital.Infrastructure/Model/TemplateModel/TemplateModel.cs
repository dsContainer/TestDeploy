using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Digital.Data.Entities;

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
        public bool isActive { get; set; }

    }
    public class TemplateViewModel
    {
        public Guid id { get; set; }    
        public string name { get; set; }
        public string normalizationName { get; set; }
        public string? description { get; set; }
        public DocumentTypeViewModel? documentType { get; set; } // đưa dạng object
        public DateTime? dateCreated { get; set; }
        public DateTime? dateUpdated { get; set; }
        public bool? isDeleted { get; set; }


    }

}
