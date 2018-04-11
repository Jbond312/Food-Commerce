using System.ComponentModel.DataAnnotations;

namespace Foods.Service.Api.Jwt.Entities
{
    public class UserAccountDto
    {
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string NewPassword { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string CurrentPassword { get; set; }
    }
}
