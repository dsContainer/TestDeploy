namespace Digital.Data.Entities
{
    public partial class Role
    {
        public Role()
        {
            RoleUsers = new HashSet<RoleUser>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizationName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RoleUser> RoleUsers { get; set; }
    }
}
