using System.Linq;
using System.Threading.Tasks;
using Common.Attributes;
using Common.Exceptions;
using FluentValidation;

namespace Common.Validation
{
    public class EntityValidator<T> : AbstractValidator<T>, IEntityValidator<T>
    {
        public async Task ValidateEntityAsync(T entity, string ruleSet = null)
        {
            var result = await this.ValidateAsync(entity, ruleSet: ruleSet);
            if (!result.IsValid)
            {
                var sensitivePropertyNames = typeof(T)
                    .GetProperties().AsEnumerable()
                    .Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(Sensitive))).Select(p => p.Name);
                throw new FoodsValidationException(result, sensitivePropertyNames);
            }
        }

        public AbstractValidator<T> Validator => this ;
    }
}
