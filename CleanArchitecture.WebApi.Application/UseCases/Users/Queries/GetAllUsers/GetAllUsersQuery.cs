using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.DTOs.Users;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery : IQuery<IReadOnlyList<UserResponse>>;