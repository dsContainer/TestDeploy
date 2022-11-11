using Digital.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Interface
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(User account);
        Task<string> GenerateTokenDMSAsync(User account);
        string GenerateToken(params Claim[] claims);
    }
}
