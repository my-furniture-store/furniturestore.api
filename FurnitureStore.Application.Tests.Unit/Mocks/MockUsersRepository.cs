using FurnitureStore.Domain.Users;

namespace FurnitureStore.Application.Tests.Unit.Mocks;

public static class MockUsersRepository
{
    public static IUsersRepository GetUsersRepository()
    {
        var mockRepo = Substitute.For<IUsersRepository>();
        var users = UsersFixture.GetUsers();

        mockRepo.GetAllUsersAsync().Returns(users);
        mockRepo.GetByIdAsync(Arg.Any<Guid>())
            .Returns(x => users.FirstOrDefault(user => user.Id == x.Arg<Guid>()));
        mockRepo.UserExists(Arg.Any<string>())
            .Returns(x => users.Any(user => user.Email == x.Arg<string>()));
        mockRepo.UsernameUsed(Arg.Any<string>())
            .Returns(x => users.Any(user => user.Username == x.Arg<string>()));
        mockRepo.CreateUserAsync(Arg.Do<User>(users.Add));
        return mockRepo;
    }
}
