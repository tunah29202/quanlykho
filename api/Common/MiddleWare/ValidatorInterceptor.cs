using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Common
{
    public class ValidatorInterceptor : IValidatorInterceptor
        {
            public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
            {
                if (!result.IsValid)
                {
                    throw new ValidationException("Validation failed.", result.Errors);
                }
                return result;
            }
            public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
            {
                return commonContext;
            }
    }
}
