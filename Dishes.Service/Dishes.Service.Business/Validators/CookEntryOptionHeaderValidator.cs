using Common.Repository;
using Common.Validation;
using Dishes.Service.Business.Entities;
using FluentValidation;
using Foods.Service.Repository.Dishes;

namespace Dishes.Service.Business.Validators
{
    public class CookEntryOptionHeaderValidator : EntityValidator<CookEntryOptionHeaderDto>
    {
        private readonly IRepository<CookEntry> _repo;
        private readonly string _cookId;

        public CookEntryOptionHeaderValidator(IRepository<CookEntry> repo, string cookId)
        {
            _repo = repo;
            _cookId = cookId;

            CreateRules();
        }

        private void CreateRules()
        {
            RuleFor(o => o.Name).NotEmpty().Length(1, 128);
            RuleFor(o => o.Options).SetCollectionValidator(x => new CookEntryOptionValidator(_repo, _cookId));
        }
    }
}
