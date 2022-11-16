using Digital.Infrastructure.Model.RoleUserModel;
using Digital.Infrastructure.Model.SignatureModel;

namespace Digital.Infrastructure.Model.UserModel
{
    public class UserModel
    {
    }

    public class UserViewDetailModel
    {
        public Guid Id { get; set; }//
        public string Email { get; set; }//
        public string Phone { get; set; }//
        public string Username { get; set; }//
        public string FullName { get; set; }//
        public string Password { get; set; }//
        public bool IsDeleted { get; set; } //
        public List<SignatureViewModel> Signature { get; set; } //
        public List<ListRoleViewModel> ListRole { get; set; }
    }
}
