using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.UseCases.Permissions.Commands.CreatePermission;

public class CreatePermissionCommandHandler : ICommandHandler<CreatePermissionCommand, Guid>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePermissionCommandHandler(
        IPermissionRepository permissionRepository,
        IUnitOfWork unitOfWork)
    {
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreatePermissionCommand command, CancellationToken ct = default)
    {
        var permission = Permission.Create(command.Name, command.Description);

        try
        {
            var response = await _permissionRepository.Add(permission);
            await _unitOfWork.SaveChangesAsync(ct);

            return response.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
