using CleanArchitecture.WebApi.Application.Abstractions.Mediator;
using CleanArchitecture.WebApi.Application.DTOs.Users;

namespace CleanArchitecture.WebApi.Application.UseCases.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<UserResponse>;