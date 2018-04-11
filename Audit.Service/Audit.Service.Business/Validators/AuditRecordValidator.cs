using Audit.Service.Business.Entities;
using Common.Validation;
using FluentValidation;

namespace Audit.Service.Business.Validators
{
    public class AuditRecordValidator : EntityValidator<AuditRecordDto>
    {
        public AuditRecordValidator()
        {
            RuleFor(u => u.Request).NotEmpty().WithMessage("Audit Record must contain a serialised Request.");
            RuleFor(u => u.Response).NotEmpty().WithMessage("Audit Record must contain a serialised Response.");
            RuleFor(u => u.CommandName).NotEmpty().WithMessage("Audit Record must be connected to a command.");
        }
    }
}