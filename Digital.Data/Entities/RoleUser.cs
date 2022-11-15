namespace Digital.Data.Entities
{
    public partial class RoleUser
    {
        public Guid RoleUserId { get; set; }
        public Guid RolesId { get; set; }
        public Guid UsersId { get; set; }

        public virtual Role Roles { get; set; }
        public virtual User Users { get; set; }
    }
}
