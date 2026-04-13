using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Exceptions;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Commands.UpdateRole;

public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork
    )
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateRoleCommand command, CancellationToken ct = default)
    {
        var role = await _roleRepository.GetByIdAsync(command.Id, ct);
        
        if(role is null)
            throw new NotFoundException();

        role.Update(command.Name);
        role.SyncPermissions(command.PermissionIds);

        try
        {
            await _roleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }
    }
}
