using FluentValidation;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
