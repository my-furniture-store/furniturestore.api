using FurnitureStore.Contracts.Products;
using FurnitureStore.Tests.Common.Fixtures;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.ProductsController;


[Collection("FurnitureStore.API Collection")]
public class GetProductsControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public GetProductsControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async Task Get_ShouldReturnProductNotFound_WhenProductDoesNotExist()
    {
        // Act
        var response = await _httpClient.GetAsync($"api/products/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);

        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Be("Product not found.");
    }

    [Fact]
    public async Task Get_ShouldReturnProduct_WhenProductExist()
    {
        //Arrange
        var category = CategoriesFixture.GetTestCategories()[1];
        var subCategory = SubCategoriesFixture.GetTestSubCategories(category.Id)[2];
        var product = ProductsFixture.GetListofProducts(category.Id, subCategory.Id)[0];

        await CategoryTestHelper.CreateCategory(_appFactory, category);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);
        await ProductTestHelper.CreateProduct(_appFactory, product);

        // Act
        var response = await _httpClient.GetAsync($"api/Products/{product.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var productResponse = await HttpResponseHelper.GetFromResponse<ProductResponse>(response);
        productResponse.Should().NotBeNull();
        productResponse.Should().BeEquivalentTo(product, options =>options
                .ExcludingMissingMembers()
                .Excluding(product => product.Status)
                );
    }


    public async Task InitializeAsync()
    {
        await ProductTestHelper.ClearAllProducts(_appFactory);
        await SubCategoryTestHelper.ClearAllSubCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
   
    public async Task DisposeAsync()
    {
        await ProductTestHelper.ClearAllProducts(_appFactory);
        await SubCategoryTestHelper.ClearAllSubCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
}
