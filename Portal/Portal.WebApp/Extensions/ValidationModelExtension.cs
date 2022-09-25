using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Portal.Application.Validation;

namespace Portal.WebApp.Extensions
{
    public static class ValidationModelExtension
    {
        public static async Task<bool> ValidateModel<TValidator, TTypeValidate>(this ErrorMessages<TValidator, TTypeValidate> errorMessages, TTypeValidate model, ModelStateDictionary? modelState)
            where TValidator : AbstractValidator<TTypeValidate>, new()
        {
            var validator = new TValidator();
            var resultValidation = await validator.ValidateAsync(model);
            if (resultValidation.IsValid)
            {
                return true;
            }

            foreach (var error in resultValidation.Errors)
            {
                modelState?.AddModelError(string.Empty, error.ErrorMessage);
            }

            return false;
        }
    }
}