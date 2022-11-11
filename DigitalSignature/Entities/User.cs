namespace DigitalSignature.Entities
{
    public partial class User
    {
        public User()
        {
            Documents = new HashSet<Document>();
            Roles = new HashSet<Role>();
        }

        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid RoleId { get; set; }
        public Guid SigId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ProcessStep? ProcessStep { get; set; }
        public virtual Signature? Signature { get; set; }
        public virtual ICollection<Document> Documents { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
