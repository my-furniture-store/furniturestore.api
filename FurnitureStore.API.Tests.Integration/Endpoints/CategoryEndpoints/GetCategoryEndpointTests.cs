using Newtonsoft.Json;

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
    public async Task Get_ReturnsEmptyResult_WhenNoCategoriesExist()
    {
        // Act
        var response = await _httpClient.GetAsync("api/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
        categories.Should().BeEmpty();
    }


    [Fact]
    public async Task GetById_ReturnsNotFound_WhenIdIsInvalid()
    {
        // Act
        var response = await _httpClient.GetAsync("api/categories/123");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async void GetById_ReturnCategory_WhenIdIsValid()
    {
        // Arrange
        var category = new Category { Name = "chairs" };
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        // Act
        var response = await _httpClient.GetAsync($"api/categories/{category.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categoryResponse = await HttpResponseHelper.GetFromResponse<CategoryDto>(response);
        categoryResponse.Should().NotBeNull();
        categoryResponse!.Should().BeEquivalentTo(category);
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
