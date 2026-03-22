using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetAllUsers;

public sealed class UserSpecification : Specification<User>
{
    public UserSpecification(string? firstName, string? lastName, bool? isActive, string? orderBy, string? orderDirection)
    {
        AsNoTracking();
        AddInclude(u => u.UserRoles);

        if (!string.IsNullOrWhiteSpace(firstName))
            AddCriteria(u => u.FirstName.Contains(firstName));

        if (!string.IsNullOrWhiteSpace(lastName))
            AddCriteria(u => u.LastName.Contains(lastName));

        if (isActive.HasValue)
            AddCriteria(u => u.IsActive == isActive.Value);

        var isDescending = orderDirection?.ToLower() == "desc";

        switch (orderBy?.ToLower())
        {
            case "firstname":
                if (isDescending) AddOrderByDescending(u => u.FirstName);
                else AddOrderBy(u => u.FirstName);
                break;
            case "lastname":
                if (isDescending) AddOrderByDescending(u => u.LastName);
                else AddOrderBy(u => u.LastName);
                break;
            case "createdat":
                if (isDescending) AddOrderByDescending(u => u.CreatedAt);
                else AddOrderBy(u => u.CreatedAt);
                break;
            default:
                AddOrderByDescending(u => u.CreatedAt);
                break;
        }
    }
}
