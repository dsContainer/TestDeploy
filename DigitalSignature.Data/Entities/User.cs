using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection.Metadata;

namespace Digital.Data.Entities
{
    public class User : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        [ForeignKey("RoleId")]
        public Guid RoleId { get; set; }
        public virtual ICollection<Role>? Roles { get; set; } = new List<Role>();
        [ForeignKey("SigId")]
        public Guid SigId { get; set; }
        public virtual Signature? Signature { get; set; }
        public virtual ICollection<Document>? Document { get; set; } = new List<Document>();
        public virtual ProcessStep? ProcessStep { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
    }
}

