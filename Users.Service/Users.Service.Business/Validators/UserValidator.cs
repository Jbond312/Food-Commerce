using System.Threading;
using System.Threading.Tasks;
using Common.Validation;
using FluentValidation;
using Foods.Service.Repository.Users;
using Foods.Service.Repository.Users.Entities;
using Users.Service.Business.Entities;

namespace Users.Service.Business.Validators
{
    public class UserValidator : EntityValidator<UserDto>
    {
        private readonly IUserRepository<User> _repo;

        public UserValidator(IUserRepository<User> repo)
        {
            _repo = repo;

            RuleFor(u => u.Id).MustAsync(NotAlreadyExist).WithMessage("The user does not exist.");
            RuleFor(u => u.Email).NotEmpty();
            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.Email).MustAsync(BeUnique).WithMessage("The given Email already exists.");
            RuleFor(u => u.Email).MustAsync(NotBeChanged).WithMessage("The Email cannot be directly altered via the API.");
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.LastName).NotEmpty();
        }

        private async Task<bool> NotBeChanged(UserDto userDto, string email, CancellationToken token)
        {
            var user = await _repo.Get(userDto.Id);
            return user == null || email.Equals(user.Email);
        }

        private async Task<bool> BeUnique(UserDto userDto, string email, CancellationToken token)
        {
            var user = await _repo.GetByEmail(email);
            return user == null || user.Id.Equals(userDto.Id);
        }

        private async Task<bool> NotAlreadyExist(string id, CancellationToken token)
        {
            var isExistingEntity = await _repo.Exists(id);
            return isExistingEntity;
        }

    }
}