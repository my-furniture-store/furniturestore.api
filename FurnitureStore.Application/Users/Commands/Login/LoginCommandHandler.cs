using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
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
        if (string.IsNullOrWhiteSpace(request.Username)) 
        {
            if (!await _usersRepository.UserExists(request.Email!))
                return Error.Conflict(description: "Invalid credentials.");
        }
        else
        {
            if (!await _usersRepository.UsernameUsed(request.Username))
                return Error.Conflict(description: "Invalid credentials.");
        }

        var user = await _usersRepository.GetByUsernameOrEmail(username: request.Username, email: request.Email);

        if (user == null)
            return Error.NotFound(description: "User not found.");


        if(!string.IsNullOrWhiteSpace(user.AccessToken))
        {
            if(_jwtProvider.GetTokenExpiryDate(user.AccessToken) < DateTime.UtcNow)
            {
                var accessToken = _jwtProvider.GenerateUserAccessToken(user);
                user.SetAccessToken(accessToken);
            }
        }

        await _usersRepository.UpdateUserAsync(user);
        await _unitofWork.CommitChangesAsync();

        return user.AccessToken!;
    }
}
