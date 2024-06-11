
using FurnitureStore.Domain.Products;
using FurnitureStore.Tests.Common.Fixtures;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.ProductsController;

[Collection("FurnitureStore.API Collection")]
public class DeleteProductsControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public DeleteProductsControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task Delete_ShouldReturnProductNotFound_WhenProductDoesNotExist()
    {
        //Act
        var response = await _httpClient.DeleteAsync($"api/products/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be( HttpStatusCode.NotFound );

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);

        problem.Should().NotBeNull();
        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Be("Product not found.");
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenProductExists()
    {
        // Arrange
        var product = await CreateProduct();

        //Act
        var response = await _httpClient.DeleteAsync($"api/products/{product.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);       
    }

    private async Task<Product> CreateProduct()
    {
        var category = CategoriesFixture.GetTestCategories()[1];
        var subCategory = SubCategoriesFixture.GetTestSubCategories(category.Id)[1];
        var product = ProductsFixture.GetListofProducts(category.Id, subCategory.Id)[2];

        await CategoryTestHelper.CreateCategory(_appFactory, category);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);
        await ProductTestHelper.CreateProduct(_appFactory, product);

        return product;
    }

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        await DbContextHelper.ClearAllTables(_appFactory);
    }
}
