using FluentValidation;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Commands.UpdateRole;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

        RuleFor(x => x.PermissionIds)
            .NotNull().WithMessage("{PropertyName} is required.");

        RuleForEach(x => x.PermissionIds)
            .NotEmpty().WithMessage("Each PermissionId must be a valid Guid.");
    }
}
