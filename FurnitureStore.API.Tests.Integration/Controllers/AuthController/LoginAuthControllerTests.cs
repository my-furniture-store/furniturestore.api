using FurnitureStore.Contracts.Authentication;
using FurnitureStore.Domain.Users;
using FurnitureStore.Tests.Common.Fixtures;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.AuthController;

[Collection("FurnitureStore.API Collection")]
public class LoginAuthControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;
    private Faker<LoginRequest> _userGenerator = new Faker<LoginRequest>()
        .CustomInstantiator(faker => new LoginRequest(
            Username: faker.Person.UserName,
            Email: faker.Person.Email,
            Password: "Pass@word1234"));
    public LoginAuthControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task Login_ShouldReturnAccessToken_WhenCredentialAreValid()
    {
        // Arrange
        var user = UsersFixture.GetUsers()[0];
        await DbContextHelper.CreateEntity<User>(_appFactory, user);

        var loginRequest = _userGenerator
                    .RuleFor(x => x.Username, user.Username)
                    .RuleFor(x => x.Email, string.Empty)
                    .RuleFor(x => x.Password, "John@Doe123")
                    .Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var token = await response.Content.ReadAsStringAsync();
        token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenCredentialAreInValid()
    {
        // Arrange      
        var loginRequest = _userGenerator
                    .RuleFor(x => x.Email, string.Empty)
                    .Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await HttpResponseHelper.ReadFromResponse<ProblemDetails>(response);
        error!.Title.Should().Be("Bad Request");
        error.Status.Should().Be(400);
        error.Detail.Should().Be("Invalid credentials.");
    }

   

    public Task InitializeAsync() => Task.CompletedTask; 
    public async Task DisposeAsync()
    {
        await DbContextHelper.ClearEntities<User>(_appFactory);
    }
}
