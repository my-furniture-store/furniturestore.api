using FurnitureStore.Contracts.Categories;
using FurnitureStore.Domain.Categories;

namespace FurnitureStore.API.Tests.Integration.Controllers.CategoriesController;

[Collection("FurnitureStore.API Collection")]
public class GetAllCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public GetAllCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async Task GetAll_ReturnsAllCategories_WhenCategoriesExist()
    {
        // Arrange
        var category1 = new Category("Chairs", id: Guid.NewGuid());
        var category2 = new Category("Beds", id: Guid.NewGuid());
        await CategoryTestHelper.CreateCategory(_appFactory, category1,category2);

        // Act
        var response = await _httpClient.GetAsync("api/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categoriesResponse = await HttpResponseHelper.ReadFromResponse<IEnumerable<CategoryResponse>>(response);
        categoriesResponse.Should().NotBeNull();
        categoriesResponse!.Count().Should().Be(2);
    }


    [Fact]
    public async void GetAll_ReturnsEmptyResult_WhenNoCategoriesExist()
    {
        // Act
        var response = await _httpClient.GetAsync("api/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categoriesResponse = await HttpResponseHelper.ReadFromResponse<IEnumerable<CategoryResponse>>(response);
        categoriesResponse!.Should().BeEmpty();
    }


    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
}
