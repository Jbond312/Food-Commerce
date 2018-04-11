using System.Threading.Tasks;
using FluentValidation;

namespace Common.Validation
{
    public interface IEntityValidator<T>
    {
        Task ValidateEntityAsync(T entity, string ruleSet = null);
        AbstractValidator<T> Validator { get; }
    }
}
