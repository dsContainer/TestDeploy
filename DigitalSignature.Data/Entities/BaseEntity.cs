using System;
namespace DigitalSignature.Data.Entities
{
    public class BaseEntity
    {
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
    }
}

