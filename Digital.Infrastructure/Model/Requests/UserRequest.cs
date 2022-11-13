namespace Digital.Infrastructure.Model.Requests
{
    public class UserRequest
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public Guid RoleId { get; set; }
        public Guid SigId { get; set; }
    }
}
