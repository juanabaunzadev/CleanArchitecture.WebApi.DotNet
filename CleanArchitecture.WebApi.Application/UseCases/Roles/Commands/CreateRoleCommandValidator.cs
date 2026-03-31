using FluentValidation;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Commands;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
