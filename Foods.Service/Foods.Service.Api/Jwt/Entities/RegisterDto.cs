using System.ComponentModel.DataAnnotations;
using Common.Attributes;

namespace Foods.Service.Api.Jwt.Entities
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [Sensitive]
        public string Password { get; set; }
    }
}
