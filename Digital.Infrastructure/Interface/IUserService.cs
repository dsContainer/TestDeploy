using Digital.Data.Entities;
using Digital.Infrastructure.Model.Requests;
using Digital.Infrastructure.Model.UserModel;

namespace Digital.Infrastructure.Interface
{
    public interface IUserService
    {
        List<UserViewDetailModel> GetUsers();
        User GetUser(Guid id);
        User CreateUser(UserCreateRequest userRequest);
        User UpdateUser(Guid id, UserRequest userRequest);
        User DeletedUser(Guid id, bool isDeleted);
    }
}
