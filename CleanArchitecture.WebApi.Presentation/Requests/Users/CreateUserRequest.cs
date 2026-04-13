namespace CleanArchitecture.WebApi.Presentation.Requests.Users;

public class CreateUserRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public List<Guid> RoleIds { get; set; } = [];
}