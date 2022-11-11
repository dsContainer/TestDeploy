using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Xml.Linq;

namespace Digital.Data.Entities
{
    public class Document : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? FileName { get; set; }
        public string? Description { get; set; }
        public string? FileExtension { get; set; }
        [ForeignKey("DocumentTypeID")]
        public Guid DocumentTypeID { get; set; }
        public virtual DocumentType? Type { get; set; }
        [ForeignKey("OwnerId")]
        public Guid OwnerId { get; set; }
        public virtual User? Owner { get; set; }
        [ForeignKey("ProcessId")]
        public Guid ProcessId { get; set; }
        public virtual Process? Process { get; set; }
    }
}

