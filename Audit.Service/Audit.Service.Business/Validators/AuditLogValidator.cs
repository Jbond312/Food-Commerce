using System.Threading;
using System.Threading.Tasks;
using Audit.Service.Business.Validators.Models;
using Common.Validation;
using FluentValidation;
using Common.Repository;
using Foods.Service.Repository.Audit;

namespace Audit.Service.Business.Validators
{
    public class AuditLogValidator : EntityValidator<AuditLogValidatorModel>
    {
        private readonly IRepository<AuditRecord> _repo;

        public AuditLogValidator(IRepository<AuditRecord> repo)
        {
            _repo = repo;
            RuleFor(l => l.LogDto.Data).NotEmpty().WithMessage("Audit Log must contain some data");
            RuleFor(l => l.AuditRecordId).MustAsync(Exist).WithMessage("Audit Record does not exist");
        }

        private async Task<bool> Exist(string id, CancellationToken token)
        {
            var isExistingEntity = await _repo.Exists(id);
            return isExistingEntity;
        }
    }
}
