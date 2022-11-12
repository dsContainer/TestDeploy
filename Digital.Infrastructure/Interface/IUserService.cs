using Digital.Data.Entities;
using Digital.Infrastructure.Model.Requests;

namespace Digital.Infrastructure.Interface
{
    public interface IUserService
    {
        List<User> GetUsers();
        User GetUser(Guid id);
        User CreateUser(UserRequest userRequest);
        User UpdateUser(Guid id, UserRequest userRequest);
        User DeletedUser(Guid id, bool isDeleted);
    }
}
