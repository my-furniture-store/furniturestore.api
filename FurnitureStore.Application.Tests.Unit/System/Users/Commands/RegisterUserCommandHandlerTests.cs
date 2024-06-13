using Bogus;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Application.Users.Commands.RegisterUser;
using FurnitureStore.Domain.Users;

namespace FurnitureStore.Application.Tests.Unit.System.Users.Commands;

public class RegisterUserCommandHandlerTests
{
    private readonly RegisterUserCommandHandler _sut;
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();
    private readonly Faker<User> _userGenerator = new Faker<User>()
        .CustomInstantiator(f => new User(f.Person.UserName, f.Person.Email, new byte[64], new byte[64]))
        .RuleFor(user => user.Username, faker => faker.Person.UserName)
        .RuleFor(user => user.Email, faker => faker.Person.Email);
        

    public RegisterUserCommandHandlerTests()
    {
        _usersRepository = MockUsersRepository.GetUsersRepository();

        _sut = new(_usersRepository, _unitofWork);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserWithSameEmailExists()
    {
        // Arrange
        var usedEmail = UsersFixture.GetUsers()[0].Email;
        var user = _userGenerator.Generate();
        var command = new RegisterUserCommand(user.Username, usedEmail, "Pass@word4");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.Conflict);
        result.Errors[0].Description.Should().Be("User already exists.");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserWithSameUsernameExists()
    {
        // Arrange
        var usedUserName = UsersFixture.GetUsers()[0].Username;
        var user = _userGenerator.Generate();
        var command = new RegisterUserCommand(usedUserName, user.Email, "Pass@word12");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.Conflict);
        result.Errors[0].Description.Should().Be("Username already in use.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenParametersAreValid()
    {
        // Arrange
        var user = _userGenerator.Generate();
        var command = new RegisterUserCommand(user.Username, user.Email, "Pass@word12");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();        
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<Success>();
        
    }
}
