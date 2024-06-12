using FluentValidation.TestHelper;
using FurnitureStore.Application.Users.Commands.Login;

namespace FurnitureStore.Application.Tests.Unit.System.Users.Validators;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _sut;
    private static string validUsername = "john";
    private static string validEmail = "johndoe@gmail.com";
    private static string validPassword = "Pass@word1234";

    public LoginCommandValidatorTests()
    {
        _sut = new();
    }
    [Fact]
    public void ShouldHaveError_WhenPasswordIsEmpty()
    {
        // Arrange
        var command = new LoginCommand(Username:validUsername,Password:"");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Password)
            .WithErrorMessage("Password is required.");
    }

    [Fact]
    public void ShouldHaveError_WhenPasswordLessThan8Characters()
    {
        // Arrange
        var command = new LoginCommand(Username: validUsername,Password:"Pass123");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Password)
            .WithErrorMessage("Password must be at least 8 characters long.");
    }

    [Fact]
    public void ShouldHaveError_WhenPasswordIsMissingUppercaseLetter()
    {
        // Arrange
        var command = new LoginCommand(Username: validUsername,Password: "pass@123");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter.");
    }

    [Fact]
    public void ShouldHaveError_WhenPasswordIsMissingLowercaseLetter()
    {
        // Arrange
        var command = new LoginCommand(Username:validUsername,Password: "PASS@123");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Password)
            .WithErrorMessage("Password must contain at least one lowercase letter.");
    }

    [Fact]
    public void ShouldHaveError_WhenPasswordIsMissingADigit()
    {
        // Arrange
        var command = new LoginCommand(Username:validUsername, Password: "PASS@Word");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Password)
            .WithErrorMessage("Password must contain at least one digit.");
    }

    [Fact]
    public void ShouldHaveError_WhenPasswordIsMissingASpecialCaseCharacter()
    {
        // Arrange
        var command = new LoginCommand(Username: validUsername, Password: "PASSWord1234");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Password)
            .WithErrorMessage("Password must contain at least one special character.");
    }


    [Fact]
    public void ShouldNotHavePasswordErrors_WhenPasswordIsValid()
    {
        // Arrange
        var command = new LoginCommand(Username: validUsername, Password: "Pass@Word1234");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Password);
    }

    [Fact]
    public void ShouldNotHaveErrors_WhenAreParametersAreValid()
    {
        // Arrange
        var command = new LoginCommand(Username: validUsername, Password: validPassword);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ShouldHaveError_WhenUsernameAndEmaiLAreEmpty()
    {
        // Arrange
        var command = new LoginCommand(Password: validPassword);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command)
            .WithErrorMessage("Username or email must be provided");
    }

    [Fact]
    public void ShouldHaveError_WhenUsernameAndEmailAreNotEmpty()
    {
        // Arrange
        var command = new LoginCommand(Password: validPassword, Username: validUsername, Email: validEmail);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command)
            .WithErrorMessage("Use only one - username or email.");
    }

    [Fact]
    public void ShouldNotHaveError_WhenPasswordIsProvidedWithUsername()
    {
        // Arrange
        var command = new LoginCommand(Password: validPassword, Username: validUsername);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public void ShouldNotHaveError_WhenPasswordIsProvidedWithEmail()
    { 
        // Arrange
        var command = new LoginCommand(Password: validPassword, Email: validEmail);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

}
