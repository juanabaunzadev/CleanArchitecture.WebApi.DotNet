using FluentValidation;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Commands;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.PermissionIds)
            .NotNull().WithMessage("{PropertyName} is required.");

        RuleForEach(x => x.PermissionIds)
            .NotEmpty().WithMessage("Each PermissionId must be a valid Guid.");
    }
}
