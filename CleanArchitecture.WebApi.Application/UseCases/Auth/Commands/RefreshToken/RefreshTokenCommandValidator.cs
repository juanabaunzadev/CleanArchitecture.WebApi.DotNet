using FluentValidation;

namespace CleanArchitecture.WebApi.Application.UseCases.Auth.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
