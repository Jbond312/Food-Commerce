using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Repository;
using Common.Validation;
using Dishes.Service.Business.Entities;
using FluentValidation;
using Foods.Service.Repository.Dishes;
using MongoDB.Driver;

namespace Dishes.Service.Business.Validators
{
    public class CookEntryOptionValidator : EntityValidator<CookEntryOptionDto>
    {
        private readonly IRepository<CookEntry> _cookEntryRepo;
        private readonly string _cookId;

        public CookEntryOptionValidator(IRepository<CookEntry> cookEntryRepo, string cookId)
        {
            _cookEntryRepo = cookEntryRepo;
            _cookId = cookId;

            CreateRules();
        }

        private void CreateRules()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            When(x => string.IsNullOrEmpty(x.CookEntryId), () =>
            {
                RuleFor(o => o.Name).NotEmpty().Length(1, 128);
                RuleFor(o => o.Description).NotEmpty().Length(1, 256);
                RuleFor(o => o.Price).GreaterThanOrEqualTo(0);
            });

            When(x => !string.IsNullOrEmpty(x.CookEntryId), () =>
            {
                RuleFor(o => o.Name).Empty();
                RuleFor(o => o.Description).Empty();
                RuleFor(o => o.Price).Equal(0);
                RuleFor(o => o.CookEntryId).MustAsync(Exist).WithMessage(x => $"The CookEntryId is invalid for option {x.Name}");
            });
        }


        private async Task<bool> Exist(CookEntryOptionDto cookEntryDto, string cookEntryId, CancellationToken token)
        {
            var filterBuild = Builders<CookEntry>.Filter;

            var filter = filterBuild.Where(x => x.Id == cookEntryId && x.CookId == _cookId);

            var result = await _cookEntryRepo.GetAll(filter);

            return result.Any();
        }
    }
}
