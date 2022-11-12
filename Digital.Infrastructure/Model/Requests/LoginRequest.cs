using System.ComponentModel.DataAnnotations;

namespace Digital.Infrastructure.Model.Requests
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
