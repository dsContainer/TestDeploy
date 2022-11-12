using Digital.Data.Entities;
using System.Security.Claims;

namespace Digital.Infrastructure.Interface
{
    public interface IJwtTokenService
    {
        string GenerateTokenUser(User account);
        Task<string> GenerateTokenDMSAsync(User account);
        string GenerateToken(params Claim[] claims);
    }
}
