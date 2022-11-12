using DigitalSignature.Entities;
using System.Security.Claims;

namespace DigitalSignature.Interface
{
    public interface IJwtTokenService
    {
        string GenerateTokenUser(User account);
        Task<string> GenerateTokenDMSAsync(User account);
        string GenerateToken(params Claim[] claims);
    }
}
