using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Common.Exceptions
{
    public class FoodsValidationException : Exception 
    {
        public IList<FoodsError> Errors { get; }

        public FoodsValidationException(ValidationResult validationResult, IEnumerable<string> sensitivePropertyNames = null)
        {
            if (sensitivePropertyNames == null)
            {
                sensitivePropertyNames = new List<string>();
            }

            Errors = validationResult.Errors.Select(x => new FoodsError
            {
                PropertyName = x.PropertyName,
                AttemptedValue = sensitivePropertyNames.Contains(x.PropertyName) ? "*" : x.AttemptedValue,
                ErrorMessage = x.ErrorMessage
            }).ToList();
        }

        public FoodsValidationException(string propertyName, string attemptedValue, string errorMessage)
        {
            Errors = new List<FoodsError>
            {
                new FoodsError
                {
                    PropertyName = propertyName,
                    AttemptedValue = attemptedValue,
                    ErrorMessage = errorMessage
                }
            };
        }

        public FoodsValidationException(IList<FoodsError> errors)
        {
            Errors = errors;
        }
    }
}
