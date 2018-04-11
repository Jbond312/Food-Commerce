using System.ComponentModel.DataAnnotations;

namespace Foods.Service.Api.Jwt.Entities
{
    public class ResetPasswordDto
    {
        [EmailAddress] public string Email { get; set; }
    }
}
;