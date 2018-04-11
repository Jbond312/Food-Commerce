using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Exceptions;
using FluentAssertions;
using Foods.Service.Repository.Users;
using Foods.Service.Repository.Users.Entities;
using NSubstitute;
using Ploeh.AutoFixture;
using Users.Service.Business.Entities;
using Users.Service.Business.Validators;
using Xunit;
// ReSharper disable PossibleMultipleEnumeration

namespace Users.Service.Tests.Validators
{
    public class UserValidatorTests
    {
        private readonly UserValidator _underTest;
        private readonly IUserRepository<User> _userRepo;

        private readonly Fixture _fixture;

        public UserValidatorTests()
        {
            _userRepo = Substitute.For<IUserRepository<User>>();

            _underTest = new UserValidator(_userRepo);

            _fixture = new Fixture();
        }

        [Fact]
        public void Throws_Exception_When_User_Does_Not_Exist()
        {
            var userDto = _fixture.Build<UserDto>()
                .With(x => x.Email, "a@b.com")
                .Create();

            _userRepo.Exists(Arg.Any<string>()).Returns(false);

            Func<Task> act = async () => await _underTest.ValidateEntityAsync(userDto);
            var fluentAssertionException = act.ShouldThrow<FoodsValidationException>();
            var foodsException = fluentAssertionException.Subject;
            foodsException.Should().NotBeNull();
            foodsException.Count().Should().Be(1);
            var foodsErrors = foodsException.First().Errors;
            foodsErrors.Count.Should().Be(1);
            foodsErrors.First().ErrorMessage.Should().Be("The user does not exist.");
        }
    }
}
