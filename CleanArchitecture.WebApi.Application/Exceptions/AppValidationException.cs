using FluentValidation.Results;

namespace CleanArchitecture.WebApi.Application.Exceptions;

public class AppValidationException : Exception
{
    public List<string> Errors { get; set; } = [];

    public AppValidationException(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Errors.Add(error.ErrorMessage);
        }
    }
}
