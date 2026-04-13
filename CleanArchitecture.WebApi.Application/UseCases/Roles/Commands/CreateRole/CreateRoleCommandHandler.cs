using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Persistence;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.UseCases.Roles.Commands;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Guid>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork
    )
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateRoleCommand command, CancellationToken ct = default)
    {
        var role = Role.Create(command.Name);
        role.SyncPermissions(command.PermissionIds);

        try
        {
            var response = await _roleRepository.Add(role);
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
