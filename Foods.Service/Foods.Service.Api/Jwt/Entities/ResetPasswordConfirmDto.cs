using System.ComponentModel.DataAnnotations;

namespace Foods.Service.Api.Jwt.Entities
{
    public class ResetPasswordConfirmDto
    {
        public string IdentityId { get; set; }
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string NewPassword { get; set; }
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }

        public string ResetToken { get; set; }
    }
}
