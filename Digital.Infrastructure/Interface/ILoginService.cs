using Digital.Data.Entities;

namespace Digital.Infrastructure.Interface
{
    public interface ILoginService
    {
        User AuthenticateUser(string userName, string password);
    }
}
