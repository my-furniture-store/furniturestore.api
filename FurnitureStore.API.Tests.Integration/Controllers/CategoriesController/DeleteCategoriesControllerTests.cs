
using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.SubCategories;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.CategoriesController;

[Collection("FurnitureStore.API Collection")]
public class DeleteCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public DeleteCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async void Delete_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Act
        var response = await _httpClient.DeleteAsync($"api/categories/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);

        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Be("Category not found.");
    }


    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenCategoryExists()
    {
        // Arrange
        var category = new Category("Chairs");
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        // Act
        var response = await _httpClient.DeleteAsync($"api/categories/{category.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ShouldReturnConflict_WhenCategoryHasAssociatedSubCategories()
    {
        // Arrange
        var category = new Category("Chairs");
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        var subCategory1 = new SubCategory("Loveseats", categoryId: category.Id);
        var subCategory2 = new SubCategory("Chesterfield Sofas", categoryId: category.Id);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory1, subCategory2);

        // Act
        var response = await _httpClient.DeleteAsync($"api/categories/{category.Id}");

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);

        problem!.Title.Should().Be("Conflict");
        problem.Status.Should().Be(409);
        problem.Detail.Should().Be("Can't delete category with associated sub-categories.");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await SubCategoryTestHelper.ClearAllCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }

}
