using ErrorOr;
using MediatR;

namespace FurnitureStore.Application.Users.Commands.Login;

public record LoginCommand(string Password,string? Username = null, string? Email = null) : IRequest<ErrorOr<string>>;

