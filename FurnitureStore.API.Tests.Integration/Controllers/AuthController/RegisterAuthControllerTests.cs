using FurnitureStore.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.AuthController;

[Collection("FurnitureStore.API Collection")]
public class RegisterAuthControllerTests
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;
    private readonly Faker<RegisterRequest> _userGenerator = new Faker<RegisterRequest>()
        .CustomInstantiator(faker => new RegisterRequest(
           Username: faker.Person.UserName,
           Email: faker.Person.Email,
           Password: "Pass@word1234"));       

    public RegisterAuthControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async Task Register_ShouldReturnValidationError_WhenUseranameIsMissing()
    {
        // Arrange
        var user = _userGenerator.RuleFor(x => x.Username, string.Empty).Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", user);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var validationError = await HttpResponseHelper.GetFromResponse<ValidationProblemDetails>(response);
        validationError!.Status.Should().Be(400);
        validationError.Title.Should().Be("One or more validation errors occurred.");
        validationError.Errors["Username"][0].Should().Be("The Username field is required.");
    }

    [Fact]
    public async Task Register_ShouldReturnValidationError_WhenEmailIsMissing()
    {
        // Arrange
        var user = _userGenerator.RuleFor(x => x.Email, string.Empty).Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", user);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var validationError = await HttpResponseHelper.GetFromResponse<ValidationProblemDetails>(response);
        validationError!.Status.Should().Be(400);
        validationError.Title.Should().Be("One or more validation errors occurred.");
        validationError.Errors["Email"][0].Should().Be("The Email field is required.");
        validationError.Errors["Email"][1].Should().Be("The Email field is not a valid e-mail address.");
    }
    
    [Fact]
    public async Task Register_ShouldReturnValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        var invalidEmail = "some_email";
        var user = _userGenerator.RuleFor(x => x.Email, invalidEmail).Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", user);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var validationError = await HttpResponseHelper.GetFromResponse<ValidationProblemDetails>(response);
        validationError!.Status.Should().Be(400);
        validationError.Title.Should().Be("One or more validation errors occurred.");
        validationError.Errors["Email"][0].Should().Be("The Email field is not a valid e-mail address.");
    }

    [Fact]
    public async Task Register_ShouldReturnValidationError_WhenPasswordIsMissing()
    {
        // Arrange
        var user = _userGenerator.RuleFor(x => x.Password, string.Empty).Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", user);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var validationError = await HttpResponseHelper.GetFromResponse<ValidationProblemDetails>(response);
        validationError!.Status.Should().Be(400);
        validationError.Title.Should().Be("One or more validation errors occurred.");
        validationError.Errors["Password"][0].Should().Be("The Password field is required.");
    }

    [Fact]
    public async Task Register_ShouldReturnValidationError_WhenPasswordIsInvalid()
    {
        // Arrange
        var invalidPassword = "some_password";
        var user = _userGenerator.RuleFor(x => x.Password, invalidPassword).Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", user);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var validationError = await HttpResponseHelper.GetFromResponse<ValidationProblemDetails>(response);
        validationError!.Status.Should().Be(400);
        validationError.Title.Should().Be("One or more validation errors occurred.");
    }

    [Fact]
    public async Task Register_ShouldReturnSuccessMessage_WhenUserIsCreated()
    {
        // Arrange
        var user = _userGenerator.Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", user);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var successMessage = await response.Content.ReadAsStringAsync();
        successMessage.Should().NotBeNullOrWhiteSpace();
    }

}
