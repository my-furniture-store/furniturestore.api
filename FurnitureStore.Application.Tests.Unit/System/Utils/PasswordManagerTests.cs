using FurnitureStore.Application.Utils;

namespace FurnitureStore.Application.Tests.Unit.System.Utils;

public class PasswordManagerTests
{

    [Fact]
    public void CreatePasswordHash_ShouldCreateNonNullHashAndSalt()
    {
        // Arrange
        string password = "testpassword";

        // Act
        PasswordManager.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        // Assert
        passwordHash.Should().NotBeNullOrEmpty();
        passwordSalt.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void CreatePasswordHash_ShouldCreateDifferentHashAndSalt_ForDifferentPasswords()
    {
        // Arrange
        string password1 = "Password1";
        string password2 = "Password2";

        //Act
        PasswordManager.CreatePasswordHash(password1, out byte[] passwordHash1, out byte[] passwordSalt1);
        PasswordManager.CreatePasswordHash(password2, out byte[] passwordHash2, out byte[] passwordSalt2);

        // Assert
        passwordHash1.Should().NotEqual(passwordHash2);
        passwordSalt1.Should().NotEqual(passwordSalt2);
    }


    [Fact]
    public void VerifyPassword_ShouldReturnTrueForCorrectPassord()
    {
        // Arrange
        string password = "testPassword";
        PasswordManager.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        // Act
        bool isValid = PasswordManager.VerifyPasswordHash(password, passwordHash, passwordSalt);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalseForInCorrectPassord()
    {
        // Arrange
        string password = "testPassword";
        string wrongPassword = "wrongPassword";
        PasswordManager.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        // Act
        bool isValid = PasswordManager.VerifyPasswordHash(wrongPassword, passwordHash, passwordSalt);

        // Assert
        isValid.Should().BeFalse();
    }   
    
}
