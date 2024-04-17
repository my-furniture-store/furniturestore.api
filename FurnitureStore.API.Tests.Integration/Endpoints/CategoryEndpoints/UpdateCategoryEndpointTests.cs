
namespace FurnitureStore.API.Tests.Integration.Endpoints.CategoryEndpoints;

[Collection("FurnitureStore.API Collection")]
public class UpdateCategoryEndpointTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    private readonly Faker<CategoryDto> _categoryGenerator = new Faker<CategoryDto>()
        .RuleFor(c => c.Name, faker => faker.Commerce.Categories(1)[0]);

    public UpdateCategoryEndpointTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async void Update_UpdatesCategory_WhenDataIsValid()
    {
        // Arrange
        var category = new Category { Name = "Test" };
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        var categoryDto = _categoryGenerator.Generate();

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/categories/{category.Id}", categoryDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
    }


    public async Task InitializeAsync()
    {
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
    public async Task DisposeAsync()
    {
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }

}
