using FurnitureStore.Contracts.Products;
using FurnitureStore.Tests.Common.Fixtures;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.CategoriesController;

[Collection("FurnitureStore.API Collection")]
public class GetAllProductsCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public GetAllProductsCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task GetProductsByCategoryId_ShouldReturnCategoryNotFound_WhenCategorySpecifiedDoesNotExist()
    {
        // Act
        var result = await _httpClient.GetAsync($"api/categories/{Guid.NewGuid()}/products");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var problem = await HttpResponseHelper.GetFromResponse<ValidationProblemDetails>(result);

        problem.Should().NotBeNull();
        problem!.Status.Should().Be(404);
        problem!.Detail.Should().Be("Category not found.");
    }

    [Fact]
    public async Task GetProductsByCategoryId_ShouldReturnEmptyList_WhenCategorySpecifiedHasNoProductsAssigned()
    {
        // Arrange
        var category = CategoriesFixture.GetTestCategories()[1];
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        // Act
        var result = await _httpClient.GetAsync($"api/categories/{category.Id}/products");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var list = await HttpResponseHelper.GetFromResponse<IEnumerable<ProductResponse>>(result);

        list.Should().BeEmpty();
    }

    [Fact]
    public async Task GetProductsByCategoryId_ShouldReturnListOfProducts_WhenCategorySpecifiedHasProductsAssigned()
    {
        // Arrange
        var category = CategoriesFixture.GetTestCategories()[1];
        var subCategory = SubCategoriesFixture.GetTestSubCategories(category.Id)[1];
        var product1 = ProductsFixture.GetListofProducts(category.Id, subCategory.Id)[0];
        var product2 = ProductsFixture.GetListofProducts(category.Id, subCategory.Id)[2];
        await CategoryTestHelper.CreateCategory(_appFactory, category);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);
        await ProductTestHelper.CreateProduct(_appFactory, product1, product2);


        // Act
        var result = await _httpClient.GetAsync($"api/categories/{category.Id}/products");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var productsList = await HttpResponseHelper.GetFromResponse<IEnumerable<ProductResponse>>(result);

        productsList.Should().NotBeEmpty();
        productsList!.Should().BeOfType<List<ProductResponse>>();
        productsList!.Count().Should().Be(2);
        productsList.Should().SatisfyRespectively(
            first =>
            {
                first.Id.Should().Be(product1.Id);
                first.Name.Should().Be(product1.Name);
            },
            second =>
            {
                second.Id.Should().Be(product2.Id);
                second.Name.Should().Be(product2.Name);
            }
            );
        productsList!.Should().AllSatisfy(product =>
        {
            product.CategoryName.Should().Be(category.Name);
        });
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync()
    {
        await DbContextHelper.ClearAllTables(_appFactory);
    }

}
