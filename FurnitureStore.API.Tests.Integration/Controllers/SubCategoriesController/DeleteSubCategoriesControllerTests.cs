
using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.SubCategories;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.SubCategoriesController;

[Collection("FurnitureStore.API Collection")]
public class DeleteSubCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public DeleteSubCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async void Handle_ShouldReturnCategoryNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var categoryTables = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, categoryTables);

        var subCategory = new SubCategory("Coffee Table", categoryId: categoryTables.Id);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);

        // Act
        var response = await _httpClient.DeleteAsync($"api/categories/{categoryId}/subcategories/{subCategory.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Be("Category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSubCategoryNotFound_WhenSubCategoryDoesNotExist()
    {
        // Arrange
        var category = new Category("Chairs");
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        // Act
        var response = await _httpClient.DeleteAsync($"api/categories/{category.Id}/subcategories/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Be("Sub-category not found.");
    }
    
    [Fact]
    public async Task Handle_ShouldReturnSubCategoryNotFound_WhenSubCategoryDoesNotExistInSpecifiedCategory()
    {
        // Arrange
        var category1 = new Category("Chairs");
        var category2 = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, category1, category2);

        var subCategory = new SubCategory("Coffee Table", categoryId: category2.Id);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);    

        // Act
        var response = await _httpClient.DeleteAsync($"api/categories/{category1.Id}/subcategories/{subCategory.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Be("Sub-category not found.");
    }


    [Fact]
    public async Task Handle_ShouldReturnNoContent_WhenSubCategoryIsDeletedFromSpecifiedCategory()
    {
        // Arrange
        var category = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        var subCategory = new SubCategory("Coffee Table", categoryId: category.Id);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);

        // Act
        var response = await _httpClient.DeleteAsync($"api/categories/{category.Id}/subcategories/{subCategory.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
       
    }


    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync()
    {
        await SubCategoryTestHelper.ClearAllSubCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }

}
