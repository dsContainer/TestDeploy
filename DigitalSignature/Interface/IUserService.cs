using DigitalSignature.Entities;
using DigitalSignature.Model.Requests;

namespace DigitalSignature.Interface
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
