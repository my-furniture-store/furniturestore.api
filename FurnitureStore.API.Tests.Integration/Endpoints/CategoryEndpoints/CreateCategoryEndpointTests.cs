namespace FurnitureStore.API.Tests.Integration.Endpoints.CategoryEndpoints;

[Collection("FurnitureStore.API Collection")]
public class CreateCategoryEndpointTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    private readonly Faker<CategoryDto> _categoryGenerator = new Faker<CategoryDto>()
        .RuleFor(c => c.Name, faker => faker.Commerce.Categories(1)[1]);

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
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var categoryresponse = await response.Content.ReadFromJsonAsync<CategoryDto>();
        categoryresponse!.Name.Should().Be(category.Name);
        response.Headers.Location!.ToString().Should()
            .Be("http://localhost/api/categories/1");
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
