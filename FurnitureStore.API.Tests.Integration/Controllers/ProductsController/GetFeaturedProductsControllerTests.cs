
using FurnitureStore.Contracts.Products;
using FurnitureStore.Tests.Common.Fixtures;

namespace FurnitureStore.API.Tests.Integration.Controllers.ProductsController;

[Collection("FurnitureStore.API Collection")]
public class GetFeaturedProductsControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public GetFeaturedProductsControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoFeaturedProductsExist()
    {
        // Act
        var response = await _httpClient.GetAsync("api/products/featured");

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
        var subCategory = SubCategoriesFixture.GetTestSubCategories(category.Id)[1];
        var product1 = ProductsFixture.GetListofProducts(category.Id, subCategory.Id)[0]; // not featured
        var product2 = ProductsFixture.GetListofProducts(category.Id, subCategory.Id)[1]; // featured        

        await CategoryTestHelper.CreateCategory(_appFactory, category);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);
        await ProductTestHelper.CreateProduct(_appFactory, product1, product2);

        // Act
        var response = await _httpClient.GetAsync("api/products/featured");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var productsResponse = await HttpResponseHelper.GetFromResponse<List<ProductResponse>>(response);

        productsResponse.Should().NotBeEmpty();
        productsResponse.Should().BeOfType<List<ProductResponse>>();
        productsResponse!.Count.Should().Be(1);
        productsResponse!.Select(product => product.IsFeatured).Should().Equal(true);
    }


    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync()
    {
        await DbContextHelper.ClearAllTables(_appFactory);
    }

}
