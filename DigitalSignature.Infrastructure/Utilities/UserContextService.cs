using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Infrastructure.Utilities
{
    public interface IUserContextService
    {
        Guid? UserID { get; }
        string? Username { get; }
        string? FullName { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public UserContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid? UserID =>
            Guid.TryParse(
                _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(cl => cl.Type == "UserId")
                    ?.Value, out var id)
                ? id
                : null;

        public string? Username =>
            _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(cl => cl.Type == "UserName")?.Value ?? null;

        public string? Email =>
            _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(cl => cl.Type == "Email")?.Value ?? null;
        public string? FullName => _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(cl => cl.Type == "LastName")?.Value + " " +
            _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(cl => cl.Type == "FirstName")?.Value ?? (_contextAccessor.HttpContext?.User.Claims.FirstOrDefault(cl => cl.Type == "FullName")?.Value ?? null);
        public bool IsAuthenticated => _contextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}