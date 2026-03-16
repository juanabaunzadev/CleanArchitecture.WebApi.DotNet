using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.Abstractions.Repositories;
using CleanArchitecture.WebApi.Application.DTOs.Users;
using CleanArchitecture.WebApi.Application.Exceptions;
using CleanArchitecture.WebApi.Application.Mappers;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> Handle(GetUserByIdQuery query)
    {
        var user = await _userRepository.GetByIdAsync(query.Id);

        if(user is null)
        {
            throw new NotFoundException();
        }

        return UserMapper.ToResponse(user);
    }
}
