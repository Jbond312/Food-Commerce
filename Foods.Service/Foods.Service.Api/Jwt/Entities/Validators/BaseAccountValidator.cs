using Common.Validation;

namespace Foods.Service.Api.Jwt.Entities.Validators
{
    public class BaseAccountValidator<T> : EntityValidator<T> where T : class
    {
        protected bool EmailHasValidLength(string email)
        {
            var emailSplit = email.Split("@");

            const int totalLengthLimit = 254;
            const int localLengthLimit = 64;
            const int domainLengthLimit = 255;

            return email.Length <= totalLengthLimit
                   && emailSplit[0].Length <= localLengthLimit
                   && emailSplit[1].Length <= domainLengthLimit;
        }
    }
}
