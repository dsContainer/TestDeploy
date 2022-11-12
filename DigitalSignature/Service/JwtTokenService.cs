using DigitalSignature.Entities;
using DigitalSignature.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DigitalSignature.Service
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        public static readonly Guid SYSTEM_ACCOUNT_ID = new("f1eaca5e-fad5-1eaf-fa11-babb1ed0b0e5");
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(params Claim[] claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));


            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenClaims = claims.ToList();

            // Add default system account id
            if (tokenClaims.All(cl => cl.Type != ClaimTypes.NameIdentifier))
            {
                tokenClaims.Add(new Claim(ClaimTypes.NameIdentifier, SYSTEM_ACCOUNT_ID.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(tokenClaims),
                Issuer = _configuration["JWT:Issuer"],
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Task<string> GenerateTokenAsync(User account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var claims = new List<Claim>
            {
                new Claim("UserName" , account.Username),
                new Claim("Email", account.Email ?? ""),
                new Claim("FullName", account.FullName),
                new Claim("UserId", account.Id.ToString())
            };

            /*foreach (var role in account.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }*/

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["JWT:Issuer"],
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public Task<string> GenerateTokenDMSAsync(User account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT-DMS:Secret"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , account.Username),
                new Claim(ClaimTypes.Email, account.Email ?? ""),
                new Claim("FullName", account.FullName),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };

            /*foreach (var role in account.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }*/

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["JWT-DMS:Issuer"],
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
