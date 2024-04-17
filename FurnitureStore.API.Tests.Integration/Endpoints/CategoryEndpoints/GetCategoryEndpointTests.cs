namespace FurnitureStore.API.Tests.Integration.Endpoints.CategoryEndpoints;

[Collection("FurnitureStore.API Collection")]
public class GetCategoryEndpointTests:IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public GetCategoryEndpointTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

   

    [Fact]
    public async Task Get_ReturnAllCategories_WhenCategoriesExist()
    {
        // Arrange
        var category1 = new Category { Name = "Chairs" };
        var category2 = new Category { Name = "Tables" };
        await CategoryTestHelper.CreateCategory(_appFactory, category1, category2);

        // Act

        var response = await _httpClient.GetAsync("api/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categories = await response.Content.ReadFromJsonAsync<List<Category>>();

        categories.Should().NotBeNull();
        categories!.Count.Should().Be(2);
    }


    [Fact]
    public async void Get_ReturnsEmptyResult_WhenNoCategoriesExist()
    {
        // Act
        var response = await _httpClient.GetAsync("api/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
        categories.Should().BeEmpty();
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
