using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Application.Utils;
using MediatR;

namespace FurnitureStore.Application.Users.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<string>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUnitofWork _unitofWork;

    public LoginCommandHandler(IUsersRepository usersRepository, IJwtProvider jwtProvider, IUnitofWork unitofWork)
    {
        _usersRepository = usersRepository;
        _jwtProvider = jwtProvider;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByUsernameOrEmail(username: request.Username, email: request.Email);

        if (user == null)
            return Error.Validation(description: "Invalid credentials.");

        // Verfiy password
        if (!PasswordManager.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return Error.Validation(description: "Invalid credentials.");

        if(!string.IsNullOrWhiteSpace(user.AccessToken))
        {
            if(_jwtProvider.GetTokenExpiryDate(user.AccessToken) < DateTime.UtcNow)
            {
                var accessToken = _jwtProvider.GenerateUserAccessToken(user);
                user.SetAccessToken(accessToken);
            }
        }
        else
        {
            var accessToken = _jwtProvider.GenerateUserAccessToken(user);
            user.SetAccessToken(accessToken);
        }

        await _usersRepository.UpdateUserAsync(user);
        await _unitofWork.CommitChangesAsync();

        return user.AccessToken!;
    }
}
