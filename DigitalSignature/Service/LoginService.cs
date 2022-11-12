using AutoMapper;
using DigitalSignature.Entities;
using DigitalSignature.Interface;

namespace DigitalSignature.Service
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
            var user = _context.Users.Where(e => e.Username.Equals(userName) && e.Password.Equals(password) && e.IsDeleted == false).FirstOrDefault();

            return user;
        }
    }
}
