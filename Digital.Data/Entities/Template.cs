using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Data.Entities
{
    public class Template : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizationName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("DocumentTypeId")]
        public Guid DocumentTypeId { get; set; }

        public virtual DocumentType? DocumentType { get; set; }

    }
}
