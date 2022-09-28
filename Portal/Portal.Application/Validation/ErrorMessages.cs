using FluentValidation;

namespace Portal.Application.Validation
{
    public class ErrorMessages<TValidator, TTypeValidate> where TValidator : AbstractValidator<TTypeValidate>, new()
    {
        public async Task<bool> Validate(TTypeValidate objectToValidate)
        {
            var resultValidation = await new TValidator().ValidateAsync(objectToValidate);
            if (!resultValidation.IsValid)
            {
                foreach (var failure in resultValidation.Errors)
                {
                    Console.WriteLine(failure);
                }

                return false;
            }

            return true;
        }
    }
}
