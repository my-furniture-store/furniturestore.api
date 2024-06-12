using FluentValidation.TestHelper;
using FurnitureStore.Application.Users.Commands.RegisterUser;

namespace FurnitureStore.Application.Tests.Unit.System.Users.Validators;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _sut;
    private static string validUsername = "john";
    private static string validEmail = "johndoe@gmail.com";
    private static string validPassword = "Pass@word1234";

    public RegisterUserCommandValidatorTests()
    {
        _sut = new();
    }


    [Fact]
    public void ShouldHaveError_WhenUsernameIsEmpty()
    {
        // Arrange
        var command = new RegisterUserCommand("", "john@doe.com", "Pass@word1234");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Username)
            .WithErrorMessage("Username is required.")
            .Only();
    }

    [Fact]
    public void ShouldHaveError_WhenUsernameIsMoreThan64Character()
    {
        // Arrange
        var username = new string('a', 65);
        var command = new RegisterUserCommand(username, "john@doe.com", "Pass@word123");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Username)
            .WithErrorMessage("Username should not exceed 64 characters.")
            .Only();

    }

    [Fact]
    public void ShouldNotHaveUsernameError_WhenNameIsValid()
    {
        // Arrange
        var comamnd = new RegisterUserCommand("John Doe", "", "");

        // Act
        var result = _sut.TestValidate(comamnd);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Username);
    }

    [Fact]
    public void ShouldHaveError_WhenEmailIsEmpty()
    {
        // Arrange
        var command = new RegisterUserCommand("john", "", "Pass@word1234");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Email)
            .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void ShouldHaveError_WhenEmailIsInvalid()
    {
        // Arrange
        var command = new RegisterUserCommand("john", "johhdoe.com", "Pass@word1234");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Email)
            .WithErrorMessage("Email should be a valid email address.")
            .Only();
    }



    [Fact]
    public void ShouldNotHaveEmailError_WhenEmailIsValid()
    {
        // Arrange
        var command = new RegisterUserCommand("", "johndoe@gmail.com", "");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Email);
    }


    [Fact]
    public void ShouldHaveError_WhenPasswordIsEmpty()
    {
        // Arrange
        var command = new RegisterUserCommand(validUsername, validEmail, "");

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
        var command = new RegisterUserCommand(validUsername, validEmail, "Pass123");

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
        var command = new RegisterUserCommand(validUsername, validEmail, "pass@123");

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
        var command = new RegisterUserCommand(validUsername, validEmail, "PASS@123");

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
        var command = new RegisterUserCommand(validUsername, validEmail, "PASS@Word");

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
        var command = new RegisterUserCommand(validUsername, validEmail, "PASSWord1234");

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
        var command = new RegisterUserCommand(validUsername, validEmail, "Pass@Word1234");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Password);
    }

    [Fact]
    public void ShouldNotHaveErrors_WhenAreParametersAreValid()
    {
        // Arrange
        var command = new RegisterUserCommand(validUsername, validEmail, validPassword);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

}
