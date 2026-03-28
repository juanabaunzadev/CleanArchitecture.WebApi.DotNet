using System.Linq.Expressions;
using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.Common;
using CleanArchitecture.WebApi.Application.DTOs.Users;
using CleanArchitecture.WebApi.Application.Mappers;
using CleanArchitecture.WebApi.Domain.Entities;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, PaginatedList<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedList<UserResponse>> Handle(GetAllUsersQuery query, CancellationToken ct = default)
    {
        Expression<Func<User, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            
            filter = u =>
                u.FirstName.ToLower().Contains(search) ||
                u.LastName.ToLower().Contains(search) ||
                u.Email.Value.ToLower().Contains(search);
        }

        var paginatedUsers = await _userRepository.GetPagedAsync(query.Page, query.PageSize, filter, ct);
        var mappedItems = UserMapper.ToResponseList(paginatedUsers.Items);

        return new PaginatedList<UserResponse>(mappedItems, paginatedUsers.Page, paginatedUsers.PageSize, paginatedUsers.TotalCount);
    }
}
