using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.DTOs.Users;
using CleanArchitecture.WebApi.Application.Mappers;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, PaginatedList<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedList<UserResponse>> Handle(GetAllUsersQuery query)
    {
        var spec = new UserSpecification(query.FirstName, query.LastName, query.IsActive, query.OrderBy, query.OrderDirection);
        var users = await _userRepository.GetPagedAsync(query.Page, query.PageSize, spec);
        var mappedItems = UserMapper.ToResponseList(users.Items);

        return new PaginatedList<UserResponse>(mappedItems, users.Page, users.PageSize, users.TotalCount);
    }
}
