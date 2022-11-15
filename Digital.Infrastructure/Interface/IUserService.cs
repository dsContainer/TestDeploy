using Digital.Data.Entities;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.Requests;

namespace Digital.Infrastructure.Interface
{
    public interface IUserService
    {
        Task<ResultModel> GetUsers();
        User GetUser(Guid id);
        User CreateUser(UserCreateRequest userRequest);
        User UpdateUser(Guid id, UserRequest userRequest);
        User DeletedUser(Guid id, bool isDeleted);
    }
}
