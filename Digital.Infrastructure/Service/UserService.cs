using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model.Requests;
using Microsoft.EntityFrameworkCore;

namespace Digital.Infrastructure.Service
{
    public class UserService : IUserService
    {
        private readonly DigitalSignatureDBContext _context;
        private readonly IMapper _mapper;
        public UserService(DigitalSignatureDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<User> GetUsers()
        {
            var users = _context.Users.Include(e=>e.RoleUsers).ThenInclude(e=>e.Roles)
                           .Include(e=>e.Signature).Include(e=>e.ProcessStep).AsNoTracking().ToList();

            return users;
        }

        public User GetUser(Guid id)
        {
            var user = _context.Users.Include(e => e.RoleUsers).ThenInclude(e => e.Roles)
                           .Include(e => e.Signature).Include(e => e.ProcessStep).AsNoTracking().FirstOrDefault(e=>e.Id==id);
            return user;
        }

        public User CreateUser(UserRequest userRequest)
        {
            var user = _mapper.Map<User>(userRequest);
            user.Id = Guid.NewGuid();
            user.DateCreated = DateTime.Now;
            user.DateUpdated = DateTime.Now;

            _context.Users.Add(user);

            userRequest.RoleIds.ForEach(roleId =>
            {
                if (!_context.Roles.Any(e => e.Id == roleId)) throw new Exception("RoleId not existed");

                RoleUser roleUser = new()   
                {
                    UsersId = user.Id,
                    RolesId = roleId
                };
                _context.RoleUsers.Add(roleUser);
            });

            _context.SaveChanges();

            return GetUser(user.Id);
        }

        public User UpdateUser(Guid id, UserRequest userRequest)
        {
            var user = _context.Users.Find(id);

            if (user != null)
            {
                user = _mapper.Map(userRequest, user);
                user.DateUpdated = DateTime.Now;

                _context.Users.Update(user);

                _context.RoleUsers.RemoveRange(_context.RoleUsers.Where(e=>e.UsersId == id));

                userRequest.RoleIds.ForEach(roleId =>
                {
                    if (!_context.Roles.Any(e => e.Id == roleId)) throw new Exception("RoleId not existed");

                    RoleUser roleUser = new()
                    {
                        UsersId = user.Id,
                        RolesId = roleId
                    };
                    _context.RoleUsers.Add(roleUser);
                });

                _context.SaveChanges();
            }

            return GetUser(user.Id);
        }

        public User DeletedUser(Guid id, bool isDeleted)
        {
            var user = _context.Users.Find(id);

            if (user != null)
            {
                user.DateUpdated = DateTime.Now;
                user.IsDeleted = isDeleted;

                _context.Users.Update(user);

                _context.SaveChanges();
            }

            return GetUser(user.Id);
        }
    }
}
