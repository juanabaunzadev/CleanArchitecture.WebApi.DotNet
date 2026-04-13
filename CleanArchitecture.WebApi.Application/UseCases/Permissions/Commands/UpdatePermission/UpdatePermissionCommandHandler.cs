using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;

namespace CleanArchitecture.WebApi.Application.UseCases.Permissions.Commands.UpdatePermission;

public class UpdatePermissionCommandHandler : ICommandHandler<UpdatePermissionCommand>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePermissionCommandHandler(
        IPermissionRepository permissionRepository,
        IUnitOfWork unitOfWork)
    {
        _permissionRepository = permissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdatePermissionCommand command, CancellationToken ct = default)
    {
        var permission = await _permissionRepository.GetByIdAsync(command.Id, ct);

        if (permission is null)
            throw new NotFoundException();

        permission.Update(command.Name, command.Description);

        try
        {
            await _permissionRepository.Update(permission);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
