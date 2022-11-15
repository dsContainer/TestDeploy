﻿using AutoMapper;
using Digital.Data.Entities;
using Digital.Infrastructure.Common;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Model;
using Digital.Infrastructure.Model.Requests;
using Digital.Infrastructure.Model.UserModel;
using Digital.Infrastructure.Model.SignatureModel;
using Digital.Infrastructure.Model.RoleUserModel;
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

        public async Task<ResultModel> GetUsers()
        {
            var result = new ResultModel();
            try
            {
                var listUser =  _context.Users.ToList();
                var listUserResult = new List<UserViewDetailModel>();
                foreach (var user in listUser) 
                { 
                    var listSignature = _context.Signatures.Where(x => x.UserId== user.Id).ToList();
                    //var listRole =  _context.Users.Where(x => x.Id == user.Id).SelectMany(x.);
                    var listRoleUser = _context.RoleUsers.Where(x => x.UsersId == user.Id).ToList();
                    var listRoleResult = new List<ListRoleViewModel>();
                    foreach (var roleUser in listRoleUser)
                    {
                        var role = _context.Roles.FirstOrDefault(x => x.Id == roleUser.RolesId);
                        var roleResult = new ListRoleViewModel
                        {
                            RoleId = role.Id,
                            RoleName = role.Name,
                            description= role.Description,
                        };
                        listRoleResult.Add(roleResult);
                    }

                    var listSigantureResult = new List<SignatureViewModel>();
                    foreach (var sig in listSignature)
                    {
                        var signature = new SignatureViewModel
                        {
                            Id = sig.Id,
                            FromDate = sig.FromDate,
                            ToDate = sig.ToDate,
                            IsDelete = sig.IsActive,
                        };
                        listSigantureResult.Add(signature);
                    }
                    var userToAdd = new UserViewDetailModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Phone = user.Phone,
                        Username = user.Username,
                        FullName = user.FullName,
                        Password = user.Password,
                        IsDeleted = user.IsActive,
                        Signature = listSigantureResult,
                        listRole = listRoleResult,
                    };
                    listUserResult.Add(userToAdd);
                }
                result.IsSuccess = true;
                result.Code = 200;
                result.ResponseSuccess = listUserResult;


            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.ResponseFailed = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }

            //var users = _context.Users.Include(e => e.RoleUsers).ThenInclude(e => e.Roles)
            //               .Include(e => e.Signature).Include(e => e.ProcessStep).AsNoTracking().ToList();

            return result;
        }

        public User GetUser(Guid id)
        {
            var user = _context.Users.Include(e => e.RoleUsers).ThenInclude(e => e.Roles)
                           .Include(e => e.Signature).AsNoTracking().FirstOrDefault(e => e.Id == id);
            return user;
        }

        public User CreateUser(UserCreateRequest userRequest)
        {
            if (_context.Users.Any(e => e.Username == userRequest.Username)) throw new Exception("Username existed");

            var user = _mapper.Map<User>(userRequest);
            user.Id = Guid.NewGuid();
            user.DateCreated = DateTime.Now;
            user.DateUpdated = DateTime.Now;
            user.Password = Encryption.GenerateMD5(userRequest.Password);

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
                if (!userRequest.Username.Equals(user.Username))
                {
                    if (_context.Users.Any(e => e.Username.Equals(userRequest.Username))) throw new Exception("Username existed");
                }

                user = _mapper.Map(userRequest, user);
                user.DateUpdated = DateTime.Now;

                _context.Users.Update(user);

                _context.RoleUsers.RemoveRange(_context.RoleUsers.Where(e => e.UsersId == id));

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
                user.IsActive = isDeleted;

                _context.Users.Update(user);

                _context.SaveChanges();
            }

            return GetUser(user.Id);
        }

    }
}
