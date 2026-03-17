using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.DTOs.Users;
using CleanArchitecture.WebApi.Application.Mappers;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IReadOnlyList<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(
        IUserRepository userRepository
    )
    {
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<UserResponse>> Handle(GetAllUsersQuery query)
    {
        var users = await _userRepository.GetAllAsync();
        
        return UserMapper.ToResponseList(users);
    }
}
