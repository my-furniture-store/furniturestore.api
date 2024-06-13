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
        mockRepo.IsUsernameUnique(Arg.Any<string>())
            .Returns(x => users.Any(user => user.Username == x.Arg<string>()));
        mockRepo.GetByUsernameOrEmail(Arg.Any<string>(), Arg.Any<string>())
            .Returns(x =>
            {
                var username = (string)x[0];
                var email = (string)x[1];
                return Task.FromResult(
                    users.FirstOrDefault(user => user.Username == username || user.Email == email));
            });
        mockRepo.CreateUserAsync(Arg.Do<User>(users.Add));
        mockRepo.UpdateUserAsync(Arg.Any<User>()).Returns(Task.CompletedTask).AndDoes(
            x =>
            {
                var user = x.Arg<User>();

                var existingUser = users.FirstOrDefault(u => u.Id == user.Id);

                if(existingUser != null)
                {
                    users.Remove(existingUser);
                    users.Add(user);
                }
            });

        return mockRepo;
    }
}
