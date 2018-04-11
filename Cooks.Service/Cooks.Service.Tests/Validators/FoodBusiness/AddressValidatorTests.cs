using System;
using System.Threading.Tasks;
using Common.Exceptions;
using Cooks.Service.Business.Entities.FoodBusiness;
using Cooks.Service.Business.Validators;
using Cooks.Service.Business.Validators.FoodBusiness;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;

namespace Cooks.Service.Tests.Validators.FoodBusiness
{
    public class AddressValidatorTests
    {
        private readonly AddressValidator _underTest;
        private readonly Fixture _fixture;
        private readonly string _expectedRuleSet = RuleSets.FoodBusinessRegistrationCompleted.ToString();

        public AddressValidatorTests()
        {
            _underTest = new AddressValidator();
            _fixture = new Fixture();    
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("NotComplete")]
        [InlineData("Other")]
        public void NoException_Thrown_When_Different_Or_Empty_Ruleset(string ruleSet)
        {
            var addressDto = _fixture.Build<AddressDto>()
                .WithAutoProperties()
                .Create();

            Func<Task> act = async () => await _underTest.ValidateEntityAsync(addressDto, ruleSet);
            act.ShouldNotThrow<FoodsValidationException>();
        }

        [Fact]
        public void Exception_Thrown_When_HouseNumber_Empty()
        {
            var addressDto = _fixture.Build<AddressDto>()
                .With(a => a.HouseNumber, null)
                .Create();

            Func<Task> act = async () => await _underTest.ValidateEntityAsync(addressDto, _expectedRuleSet);
            act.ShouldThrow<FoodsValidationException>();
        }

        [Fact]
        public void Exception_Thrown_When_Line1_Empty()
        {
            var addressDto = _fixture.Build<AddressDto>()
                .With(a => a.Line1, null)
                .Create();

            Func<Task> act = async () => await _underTest.ValidateEntityAsync(addressDto, _expectedRuleSet);
            act.ShouldThrow<FoodsValidationException>();
        }

        [Fact]
        public void Exception_Thrown_When_PostCode_Empty()
        {
            var addressDto = _fixture.Build<AddressDto>()
                .With(a => a.PostCode, null)
                .Create();

            Func<Task> act = async () => await _underTest.ValidateEntityAsync(addressDto, _expectedRuleSet);
            act.ShouldThrow<FoodsValidationException>();
        }
    }
}
