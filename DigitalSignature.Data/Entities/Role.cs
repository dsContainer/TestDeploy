using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital.Data.Entities
{
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizationName { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();

    }
}

