using FluentAssertions;
using FluentAssertions.Extensions;
using FurnitureStore.Domain.Users;
using FurnitureStore.Tests.Common.Helpers;

namespace FurnitureStore.Domain.Tests.Unit.System.Users;

public class UserTests
{
    private readonly User _sut;

    public UserTests()
    {
        _sut = new User("John Doe", "johndoe@hotmail.com", new byte[32], new byte[32]);
    }


    [Fact]
    public void Constructor_ShouldIntializeUserCorrectly()
    {
        // Arrange
        var username = "Jane Doe";
        var email = "janedoe@gmail.com";
        var passwordHash = new byte[32];
        var passwordSalt = new byte[32];


        // Act
        User user = new(username, email, passwordHash, passwordSalt);

        // Assert
        user.Username.Should().Be(username);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Equal(passwordHash);
        user.PasswordSalt.Should().Equal(passwordSalt);
    }

    [Fact]
    public void SetAccessToken_ShouldSetAccessToken_WhenAccessTokenIsProvided()
    {
        // Arrange
        var accessToken = "newAccessToken";

        //Act
        _sut.SetAccessToken(accessToken);

        // Assert
        _sut.AccessToken.Should().Be(accessToken);
    }

    [Fact]
    public void SetAccessToken_ShouldNotSetAccessToken_WhenAccessTokenIsEmptyOrNull()
    {
        // Arrange
        var accessToken = string.Empty;

        // Act
        _sut.SetAccessToken(accessToken);

        // Assert
        _sut.AccessToken.Should().BeNullOrWhiteSpace();
    }


    [Fact]
    public void Verify_ShouldSetVerifiedAt_WhenVerifiedAtHasNotBeenSet()
    {
        // Act
        _sut.Verify();

        // Assert
        _sut.VerifiedAt.Should().BeWithin(10.Seconds()).Before(DateTime.Now);
    }

    [Fact]
    public void Verify_ShouldNotChangeVerifiedAt_WhenVerifiedAtIsAlreadySet()
    {
        // Arrange
        var verifiedAt = new DateTime(2000, 01, 01);
        EntityTestHelper.SetValueForPrivateSetter<User, DateTime>(_sut, verifiedAt, nameof(User.VerifiedAt));

        // Act
        _sut.Verify();

        // Assert
        _sut.VerifiedAt.Should().HaveYear(2000);
        _sut.VerifiedAt.Should().HaveMonth(1);
        _sut.VerifiedAt.Should().HaveDay(1);
    }

    [Fact]
    public void SetPasswordResetToken_ShouldSetPasswordResetToken_WhenTokenIsProvided()
    {
        // Arrange
        var accessToken = "newResetToken";

        //Act
        _sut.SetPasswordResetToken(accessToken);

        // Assert
        _sut.PasswordResetToken.Should().Be(accessToken);
    }

    [Fact]
    public void SetPasswordResetToken_ShouldNotSetPasswordResetToken_WhenTokenIsEmptyOrNull()
    {
        // Arrange
        var accessToken = string.Empty;

        // Act
        _sut.SetPasswordResetToken(accessToken);

        // Assert
        _sut.AccessToken.Should().BeNullOrWhiteSpace();
    }

    [Fact]
    public void SetResetTokenExpiryDate_ShouldSetResetTokenExpiresTwoHoursFromNow_WhenCalled()
    {
        // Act
        _sut.SetResetTokenExpiryDate();

        // Assert
        _sut.ResetTokenExpires.Should().BeWithin(2.Hours()).After(DateTime.Now);
    }


    [Fact]
    public void UpdatePassword_ShouldUpdatePasswordHashAndSalt_WhenInputsAreValid()
    {
        // Arrange
        var newPasswordHash = new byte[64];
        var newPasswordSalt = new byte[64];
        new Random().NextBytes(newPasswordHash);
        new Random().NextBytes(newPasswordSalt);

        // Act
        var result = _sut.UpdatePassword(newPasswordHash, newPasswordSalt);

        // Assert
        result.IsError.Should().BeFalse();
        _sut.PasswordHash.Should().Equal(newPasswordHash);
        _sut.PasswordSalt.Should().Equal(newPasswordSalt);
    }

    [Fact]
    public void UpdatePassword_ShouldReturnInvalidPasswordHash_WhenPasswordHashIsNull()
    {
        // Arrange
        byte[]? passwordHash = null;
        var newPasswordSalt = new byte[64];

        // Act
        var result = _sut.UpdatePassword(passwordHash, newPasswordSalt);

        // Act
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Description.Should().Be("Invalid password hash.");
    }
    
    [Fact]
    public void UpdatePassword_ShouldReturnInvalidPasswordHash_WhenPasswordHashIsInvalidLength()
    {
        // Arrange
        var newPasswordHash = new byte[32];
        var newPasswordSalt = new byte[64];

        // Act
        var result = _sut.UpdatePassword(newPasswordHash, newPasswordSalt);

        // Act
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Description.Should().Be("Invalid password hash.");
    }
    
    [Fact]
    public void UpdatePassword_ShouldReturnInvalidPasswordSalt_WhenPasswordSaltIsNull()
    {
        // Arrange
        var newPasswordHash = new byte[64];
        byte[]? passwordSalt = null;

        // Act
        var result = _sut.UpdatePassword(newPasswordHash, passwordSalt);

        // Act
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Description.Should().Be("Invalid password salt.");
    }

    [Fact]
    public void UpdatePassword_ShouldReturnInvalidPasswordSalt_WhenPasswordSaltIsInvalidLength()
    {
        // Arrange
        var newPasswordHash = new byte[64];
        var newPasswordSalt = new byte[32];

        // Act
        var result = _sut.UpdatePassword(newPasswordHash, newPasswordSalt);

        // Act
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Description.Should().Be("Invalid password salt.");
    }



}
