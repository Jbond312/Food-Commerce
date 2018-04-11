using System.Threading.Tasks;

namespace Foods.Service.Api.Jwt.Helpers
{
    public interface IAccountHelper
    {
        Task ValidateCurrentPasswordIsCorrect(FoodsIdentityUser user, string passwordToValidate);
        Task ChangePassword(FoodsIdentityUser user, string oldPassword, string newPassword);
        Task ChangeEmail(FoodsIdentityUser user, string newEmail);
        Task SendEmailConfirmation(FoodsIdentityUser user);
        Task SendResetPasswordEmail(string email);
        Task SendResetPasswordSuccessEmail(FoodsIdentityUser user);
    }
}