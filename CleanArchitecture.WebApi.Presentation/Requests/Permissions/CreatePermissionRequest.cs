namespace CleanArchitecture.WebApi.Presentation.Requests.Permissions;

public class CreatePermissionRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
