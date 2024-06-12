using FluentValidation;

namespace FurnitureStore.Application.Users.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(command => command.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");

        RuleFor(command => command)
            .Must(command => !string.IsNullOrWhiteSpace(command.Username) || !string.IsNullOrWhiteSpace(command.Email))
            .WithMessage("Username or email must be provided")
            .Must(command => string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Email))
            .WithMessage("Use only one - username or email.");

    }
}
