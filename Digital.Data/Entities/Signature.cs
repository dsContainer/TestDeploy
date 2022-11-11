using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital.Data.Entities
{
    public class Signature
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsDelete { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

    }
}

