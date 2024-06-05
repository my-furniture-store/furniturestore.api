
using FurnitureStore.Contracts.SubCategories;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.SubCategories;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.SubCategoriesController;

[Collection("FurnitureStore.API Collection")]
public class GetAllSubCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public GetAllSubCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnCategoryNotFound_WhenCategorySpecifiedDoesNotExist()
    {
        // Act
        var response = await _httpClient.GetAsync($"api/categories/{Guid.NewGuid()}/subcategories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Be("Category not found.");
    }

    [Fact]
    public async Task GetAll_ShouldReturnSubCategoriesList_WhenSubCategoriesExistInSpecifiedCategory()
    {
        // Arrange
        var category = new Category("Chairs");
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        var subCategory1 = new SubCategory("Loveseats", categoryId: category.Id);
        var subCategory2 = new SubCategory("Chesterfield Sofas", categoryId: category.Id);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory1, subCategory2);

        // Act
        var response = await _httpClient.GetAsync($"api/categories/{category.Id}/subcategories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var subCategoriesResponse = await HttpResponseHelper.ReadFromResponse<List<SubCategoryResponse>>(response);

        subCategoriesResponse.Should().NotBeNull(); 
        subCategoriesResponse!.Count.Should().Be(2);
        subCategoriesResponse.Should().BeOfType<List<SubCategoryResponse>>();
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenSubCategoriesDontExistInSpecifiedCategory()
    {
        // Arrange
        var category = new Category("Chairs");
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        // Act
        var response = await _httpClient.GetAsync($"api/categories/{category.Id}/subcategories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK );

        var subCategoriesResponse = await HttpResponseHelper.ReadFromResponse<List<SubCategoryResponse>>(response);

        subCategoriesResponse.Should().BeEmpty();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync()
    {
        await SubCategoryTestHelper.ClearAllSubCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }

}
