
using FurnitureStore.Contracts.SubCategories;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.SubCategories;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.SubCategoriesController;

[Collection("FurnitureStore.API Collection")]
public class UpdateSubCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public UpdateSubCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task Update_ShouldReturnValidationError_WhenNameIsNotProvided()
    {
        // Arrange
        var category = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, category);
        var subCategory = new SubCategory("Coffee Table", category.Id);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);

        var requestPayload = new UpdateSubCategoryRequest("");

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/categories/{category.Id}/subcategories/{subCategory.Id}", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        error!.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Name"][0].Should().Be("The Name field is required.");
    }

    [Fact]
    public async Task Update_ShouldReturnValidationError_WhenNameFieldIs25OrMoreCharacters()
    {
        // Arrange
        var category = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, category);
        var subCategory = new SubCategory("Coffee Table", category.Id);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);

        var requestPayload = new UpdateSubCategoryRequest(new string('A', 26));

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/categories/{category.Id}/subcategories/{subCategory.Id}", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        error!.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Name"][0].Should().Be("The field Name must be a string with a maximum length of 25.");
    }

    [Fact]
    public async Task Update_ShouldReturnCategoryNotFound_WhenCategorySpecifiedDoesNotExist()
    {
        // Arrange
        var category = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, category);
        var subCategory = new SubCategory("Coffee Table", category.Id);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/categories/{Guid.NewGuid()}/subcategories/{subCategory.Id}", new UpdateSubCategoryRequest("Coffee Table"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var error = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        error!.Status.Should().Be(404);
        error.Title.Should().Be("Not Found");
        error.Detail.Should().Be("Category not found.");
    }
    
    [Fact]
    public async Task Update_ShouldReturnSubCategoryNotFound_WhenSubCategorySpecifiedDoesNotExist()
    {
        // Arrange
        var category = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/categories/{Guid.NewGuid()}/subcategories/{Guid.NewGuid()}", new UpdateSubCategoryRequest("Coffee Table"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var error = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        error!.Status.Should().Be(404);
        error.Title.Should().Be("Not Found");
        error.Detail.Should().Be("Sub-category not found.");
    }

    [Fact]
    public async Task Update_ShouldUpdateSubCategory_WhenDataProvidedIsValidAndCategoryAndSubCategoryExist()
    {
        // Arrange
        var category = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, category);
        var subCategory = new SubCategory("Coffee Table", category.Id);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);

        var newName = "Dining Table";
        var requestPayload = new UpdateSubCategoryRequest(newName);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/categories/{category.Id}/subcategories/{subCategory.Id}", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var subCategoryResponse = await HttpResponseHelper.ReadFromResponse<SubCategoryResponse>(response);
        subCategoryResponse.Should().NotBeNull();
        subCategoryResponse!.SubCategoryId.Should().Be(subCategory.Id);
        subCategoryResponse!.Name.Should().Be(newName);
    }

    public Task InitializeAsync()=> Task.CompletedTask;
    public async Task DisposeAsync()
    {
        await SubCategoryTestHelper.ClearAllSubCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
}
