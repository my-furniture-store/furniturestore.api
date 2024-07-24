using ErrorOr;
using MediatR;

namespace FurnitureStore.Application.Authentication.Commands.RegisterUser;

public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<ErrorOr<Success>>;
