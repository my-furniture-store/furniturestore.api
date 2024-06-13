using FurnitureStore.Domain.Users;

namespace FurnitureStore.Application.Tests.Unit.Mocks;

public static class MockJwtProvider
{
    public static IJwtProvider GetJwtProvider()
    {
        var mockProvider = Substitute.For<IJwtProvider>();
        var users = UsersFixture.GetUsers();

        mockProvider.GenerateUserAccessToken(Arg.Any<User>())
            .Returns(x => $"token_for_{x.Arg<User>().Username}");

        mockProvider.GetTokenExpiryDate(Arg.Any<string>())
            .Returns(x =>
            {
                var token = x.Arg<string>();
                return users.Any(user => token.Contains(user.Username))
                ? DateTime.UtcNow.AddHours(1) : (DateTime?)null;
            });

        return mockProvider;
    }

}
