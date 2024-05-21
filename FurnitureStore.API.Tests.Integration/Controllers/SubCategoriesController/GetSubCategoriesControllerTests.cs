
using FurnitureStore.Contracts.SubCategories;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.SubCategories;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.SubCategoriesController;

[Collection("FurnitureStore.API Collection")]
public class GetSubCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public GetSubCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async void Get_ShouldReturnCategoryNotFound_WhenCategorySpecifiedDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        // Act
        var response = await _httpClient.GetAsync($"api/categories/{categoryId}/subcategories/{Guid.NewGuid()}");

        // Assert
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);

        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Be("Category not found.");
    }

    [Fact]                              
    public async Task Get_ShouldReturnSubCategoryNotFound_WhenSubCategoryDoesNotExist()
    {
        // Arrange
        var category = new Category("Chairs");
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        // Act
        var response = await _httpClient.GetAsync($"api/categories/{category.Id}/subcategories/{Guid.NewGuid()}");

        // Assert
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);

        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Be("Sub-category not found.");
    }

    [Fact]
    public async Task Get_ShouldReturnSubCategory_WhenSubCategoryExistsInSpecifiedCategory()
    {
        // Arrange
        var category = new Category("Chairs");
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        var subCategory = new SubCategory("Loveseats", categoryId: category.Id);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);

        // Act
        var response = await _httpClient.GetAsync($"api/categories/{category.Id}/subcategories/{subCategory.Id}");

        // Assert
        response!.StatusCode.Should().Be(HttpStatusCode.OK);

        var subCategoryResponse = await HttpResponseHelper.ReadFromResponse<SubCategoryResponse>(response);
        subCategoryResponse.Should().NotBeNull();
        subCategoryResponse.Should().BeOfType<SubCategoryResponse>();
        subCategoryResponse!.Name.Should().Be(subCategory.Name);       
    }


    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await SubCategoryTestHelper.ClearAllCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
}
