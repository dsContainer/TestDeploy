namespace Digital.Infrastructure.Interface
{
    public interface IOTPService
    {
        public Task SendEmailMessage();
        public Task ConfirmCode(string verificationCode);
    }
}

