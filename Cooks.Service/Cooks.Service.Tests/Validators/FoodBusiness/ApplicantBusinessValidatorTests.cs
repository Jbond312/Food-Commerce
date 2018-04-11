using System;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Validation;
using Cooks.Service.Business.Entities.FoodBusiness;
using Cooks.Service.Business.Validators.FoodBusiness;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture;
using Xunit;

namespace Cooks.Service.Tests.Validators.FoodBusiness
{
    public class ApplicantBusinessValidatorTests
    {
        private readonly ApplicantBusinessValidator _underTest;
        private readonly Fixture _fixture;

        public ApplicantBusinessValidatorTests()
        {
            var addressValidator = Substitute.For<EntityValidator<AddressDto>>();

            _underTest = new ApplicantBusinessValidator(addressValidator);

            _fixture = new Fixture();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Other")]
        [InlineData("FoodBusinssRegistrationCompleted")]
        public void When_IsRegisteredWithCompaniesHouse_No_RegistrationNumber_Allowed(string ruleSet)
        {
            var applicantBusinessDto = _fixture.Build<ApplicantBusinessDto>()
                .With(x => x.IsRegisteredWithCompaniesHouse, true)
                .Without(x => x.RegistrationNumber)
                .Create();

            Func<Task> act = async () => await _underTest.ValidateEntityAsync(applicantBusinessDto, ruleSet);

            act.ShouldNotThrow<FoodsValidationException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Other")]
        public void When_IsBusinessRegisteredOutsideUk_No_CommercialRegister_Allowed(string ruleSet)
        {
            var applicantBusinessDto = _fixture.Build<ApplicantBusinessDto>()
                .With(x => x.IsBusinessRegisteredOutsideUk, true)
                .Without(x => x.CommercialRegister)
                .Create();

            Func<Task> act = async () => await _underTest.ValidateEntityAsync(applicantBusinessDto, ruleSet);

            act.ShouldNotThrow<FoodsValidationException>();
        }
    }
}
