using Bogus;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Application.Users.Commands.Login;
using FurnitureStore.Domain.Users;

namespace FurnitureStore.Application.Tests.Unit.System.Users.Commands;

public class LoginCommandHandlerTests
{
    private readonly LoginCommandHandler _sut;
    private readonly IUsersRepository _usersRepository = MockUsersRepository.GetUsersRepository();
    private readonly IJwtProvider _jwtProvider = MockJwtProvider.GetJwtProvider();
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();
    private readonly Faker<User> _userGenerator = new Faker<User>()
       .CustomInstantiator(f => new User(f.Person.UserName, f.Person.Email, new byte[64], new byte[64]))
       .RuleFor(user => user.Username, faker => faker.Person.UserName)
       .RuleFor(user => user.Email, faker => faker.Person.Email);

    public LoginCommandHandlerTests()
    {
        _sut = new(_usersRepository, _jwtProvider, _unitofWork);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserWithProvidedEmailDoesNotExist()
    {
        // Arrange
        var user = _userGenerator.Generate();

        var command = new LoginCommand("Pass@word1", Email: user.Email);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.Validation);
        result.Errors[0].Description.Should().Be("Invalid credentials.");
    }
    
    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserWithProvidedUsernameDoesNotExist()
    {
        // Arrange
        var user = _userGenerator.Generate();

        var command = new LoginCommand("Pass@word1", Username: user.Username);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.Validation);
        result.Errors[0].Description.Should().Be("Invalid credentials.");
    }

    [Fact]
    public async Task Hadle_ShouldReturnError_WhenPasswordIsWrong()
    {       
        // Arrange
        var user = UsersFixture.GetUsers()[0];
        var command = new LoginCommand("Pass@word1", Username: user.Username);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.Validation);
        result.Errors[0].Description.Should().Be("Invalid credentials.");
    }

    [Fact]
    public async Task Handle_ShouldCallGenerateToken_WhenCredentialsWithUsernameAreValid()
    {
        // Arrange
        var user = UsersFixture.GetUsers()[0];
        var command = new LoginCommand("John@Doe123", Username: user.Username);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        _jwtProvider.Received(1).GenerateUserAccessToken(Arg.Is<User>(u => u.Username == user.Username));

    }
    
    [Fact]
    public async Task Handle_ShouldCallGenerateToken_WhenCredentialWithEmailAreValid()
    {
        // Arrange
        var user = UsersFixture.GetUsers()[0];
        var command = new LoginCommand("John@Doe123", Email: user.Email);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        _jwtProvider.Received(1).GenerateUserAccessToken(Arg.Is<User>(u => u.Email == user.Email));
    }


    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialAreValid()
    {
        // Arrange
        var user = UsersFixture.GetUsers()[0];
        var command = new LoginCommand("John@Doe123", Email: user.Email);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNullOrWhiteSpace();
    }
}
