using DigitalSignature.Entities;

namespace DigitalSignature.Interface
{
    public interface ILoginService
    {
        User AuthenticateUser(string userName, string password);
    }
}
