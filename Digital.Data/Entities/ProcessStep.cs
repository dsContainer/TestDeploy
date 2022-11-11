using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Digital.Data.Entities
{
    public class ProcessStep : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public float? OrderIndex { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
        public float XPoint { get; set; }
        public float YPoint { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int PageSign { get; set; }
        public DateTime DateSign { get; set; }
        public string? Message { get; set; }
        [ForeignKey("ProcessId")]
        public Guid? ProcessId { get; set; }
        public virtual Process? Process { get; set; }





    }
}
