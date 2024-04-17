using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Endpoints.CategoryEndpoints;

[Collection("FurnitureStore.API Collection")]
public class CreateCategoryEndpointTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    private readonly Faker<CategoryDto> _categoryGenerator = new Faker<CategoryDto>()
        .RuleFor(c => c.Name, faker => faker.Commerce.Categories(4)[1]);

    public CreateCategoryEndpointTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async void Create_CreatesCategory_WhenCategoryIsValid()
    {
        // Arrange
        var category = _categoryGenerator.Generate();

        // Act

        var response = await _httpClient.PostAsJsonAsync("api/categories", category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
       
        var categoryresponse = await HttpResponseHelper.GetFromResponse<CategoryDto>(response);
        categoryresponse!.Should().BeEquivalentTo(category);
        response.Headers.Location!.ToString().Should()
            .Be($"http://localhost/api/categories/{categoryresponse!.Id}");
    }

    [Fact]
    public async Task Create_ReturnValidationError_WhenNameIsEmpty()
    {
        // Arrange
        var categoryDto = new CategoryDto { Name = string.Empty };

        // Act

        var response = await _httpClient.PostAsJsonAsync("api/categories", categoryDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error!.Errors["Name"][0].Should().Be("The Name field is required.");
    }


    [Fact]
    public async Task Create_ReturnValidationError_WhenNameIsTooLong()
    {
        // Arrange
        var categoryDto = _categoryGenerator.Clone()
            .RuleFor(c => c.Name, faker => (faker.Commerce.Categories(1)[0] + faker.Commerce.ProductDescription()))
            .Generate();

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/categories", categoryDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error!.Title.Should().Be("One or more validation errors occurred.");
        error!.Errors["Name"][0].Should().Be("The field Name must be a string with a maximum length of 20.");
    }


    public async Task DisposeAsync()
    {
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }

    public async Task InitializeAsync()
    {
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
}
