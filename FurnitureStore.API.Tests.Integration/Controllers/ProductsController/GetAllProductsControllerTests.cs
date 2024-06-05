
using FurnitureStore.Contracts.Products;
using FurnitureStore.Tests.Common.Fixtures;

namespace FurnitureStore.API.Tests.Integration.Controllers.ProductsController;

[Collection("FurnitureStore.API Collection")]
public class GetAllProductsControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public GetAllProductsControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        // Act
        var response = await _httpClient.GetAsync("api/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var productsResponse = await HttpResponseHelper.GetFromResponse<List<ProductResponse>>(response);

        productsResponse.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_ShouldReturnProductsList_WhenProductsExist()
    {
        // Arrange
        var category = CategoriesFixture.GetTestCategories()[1];
        var subCategory = SubCategoriesFixture.GetTestSubCategories(category.Id)[2];
        var product1 = ProductsFixture.GetListofProducts(category.Id, subCategory.Id)[0];
        var product2 = ProductsFixture.GetListofProducts(category.Id, subCategory.Id)[1];

        await CategoryTestHelper.CreateCategory(_appFactory, category);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);
        await ProductTestHelper.CreateProduct(_appFactory, product1, product2);

        // Act
        var response = await _httpClient.GetAsync("api/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var productsResponse = await HttpResponseHelper.GetFromResponse<List<ProductResponse>>(response);

        productsResponse.Should().NotBeEmpty();
        productsResponse.Should().BeOfType<List<ProductResponse>>();
        productsResponse!.Count.Should().Be(2);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await ProductTestHelper.ClearAllProducts(_appFactory);
        await SubCategoryTestHelper.ClearAllSubCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
}
