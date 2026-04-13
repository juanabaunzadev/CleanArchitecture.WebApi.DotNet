namespace CleanArchitecture.WebApi.Presentation.Requests.Roles;

public class CreateRoleRequest
{
    public string Name { get; set; } = null!;
    public List<Guid> PermissionIds { get; set; } = [];
}
