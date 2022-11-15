using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Common;
using Digital.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace Digital.Infrastructure.Service
{
    public class LoginService : ILoginService
    {
        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;
        public LoginService(DigitalSignatureDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public User AuthenticateUser(string userName, string password)
        {
            var user = _context.Users
                           .Include(e => e.RoleUsers).ThenInclude(e => e.Roles)
                           .Include(e => e.Signature)
                           .FirstOrDefault(e => e.Username.Equals(userName) && e.IsActive == true);

            if (user != null)
            {
                if (!Encryption.GenerateMD5(password).Equals(user.Password)) user = null;
            }

            return user;
        }
    }
}
