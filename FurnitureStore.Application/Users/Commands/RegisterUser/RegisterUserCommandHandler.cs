using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Application.Utils;
using FurnitureStore.Domain.Users;
using MediatR;

namespace FurnitureStore.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<User>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitofWork _unitofWork;
    public RegisterUserCommandHandler(IUsersRepository usersRepository, IUnitofWork unitofWork)
    {
        _usersRepository = usersRepository;
        _unitofWork = unitofWork;
    }
    public async Task<ErrorOr<User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _usersRepository.UserExists(request.Email))
            return Error.Conflict(description: "User already exists.");

        if (await _usersRepository.UsernameUsed(request.Username))
            return Error.Conflict(description: "Username already in use.");

        PasswordManager.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User(
            username: request.Username,
            email: request.Email,
            passwordHash: passwordHash,
            passwordSalt: passwordSalt);

        await _usersRepository.CreateUserAsync(user);
        await _unitofWork.CommitChangesAsync();

        return user;
    }
}
