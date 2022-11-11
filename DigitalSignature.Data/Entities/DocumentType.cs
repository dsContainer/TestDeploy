using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital.Data.Entities
{
    public class DocumentType : BaseEntity
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizationName { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Document>? Documents { get; set; } = new List<Document>();
        public virtual ICollection<Template>? Template { get; set; } = new List<Template>();
    }
}

