﻿namespace Digital.Infrastructure.Interface
{
    public interface IUserContextService
    {
        Guid? UserID { get; }
        string? Username { get; }
        string? FullName { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
}
