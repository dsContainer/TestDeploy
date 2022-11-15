namespace Digital.Data.Entities
{
    public partial class User
    {
        public User()
        {
            Documents = new HashSet<Document>();
            RoleUsers = new HashSet<RoleUser>();
        }

        public Guid Id { get; set; }//
        public string Email { get; set; }//
        public string Phone { get; set; }//
        public string Username { get; set; }//
        public string FullName { get; set; }//
        public string Password { get; set; }//
        public string ImgUrl { get; set; }//
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; } //


        public virtual ICollection<Signature> Signature { get; set; }//
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<RoleUser> RoleUsers { get; set; }//
    }
}
