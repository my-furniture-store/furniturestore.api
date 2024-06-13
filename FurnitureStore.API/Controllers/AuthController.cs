using FurnitureStore.Application.Users.Commands.Login;
using FurnitureStore.Application.Users.Commands.RegisterUser;
using FurnitureStore.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Controllers;

[Route("api/auth")]
public class AuthController: ApiController
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.Username, request.Email, request.Password);

        var registerResult = await _mediator.Send(command);

        return registerResult.Match<IActionResult>(_ => Ok("User registered!"), errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = new LoginCommand(Password: request.Password, Username: request.Username, Email: request.Email);

        var loginResult = await _mediator.Send(command);

        return loginResult.MatchFirst(
            accessToken => Ok(accessToken),
            errors => Problem(errors));
    }
}
