using System.ComponentModel.DataAnnotations;

namespace Digital.Infrastructure.Model.Requests
{
    public class UserRequest
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The {0} must be between {2} and {1} character in length")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The {0} must be between {2} and {1} character in length")]
        public string FullName { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public List<Guid> RoleIds { get; set; }

        [Required]
        public Guid SigId { get; set; }
    }

    public class UserCreateRequest : UserRequest
    {
        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,20}$", ErrorMessage = "The {0} must be between 8 to 20 characters which contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character")]
        public string Password { get; set; }
    }
}
