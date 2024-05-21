using FurnitureStore.Contracts.SubCategories;
using FurnitureStore.Domain.Categories;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.SubCategoriesController;

[Collection("FurnitureStore.API Collection")]
public class CreateSubCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public CreateSubCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task Create_ShouldReturnValidationError_WhenNameIsNotProvided()
    {
        // Arrange
        var category = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, category);
        var requestPayload = new CreateSubCategoryRequest("");

        // Act
        var response = await _httpClient.PostAsJsonAsync($"api/categories/{category.Id}/subcategories", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        error!.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Name"][0].Should().Be("The Name field is required.");
    }

    [Fact]
    public async Task Create_ShouldReturnCategoryNotFound_WhenCategorySpecifiedDoesNotExist()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync($"api/categories/{Guid.NewGuid()}/subcategories", new CreateSubCategoryRequest("Coffee Table"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var error = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        error!.Status.Should().Be(404);
        error.Title.Should().Be("Not Found");
        error.Detail.Should().Be("Category not found.");
    }

    [Fact]
    public async Task Create_ShouldReturnValidationError_WhenNameFieldIs25OrMoreCharacters()
    {
        // Arrange
        var category = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, category);
        var requestPayload = new CreateSubCategoryRequest(new string('A', 26));

        // Act
        var response = await _httpClient.PostAsJsonAsync($"api/categories/{category.Id}/subcategories", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        error!.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Name"][0].Should().Be("The field Name must be a string with a maximum length of 25.");
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedSubCategory_WhenNameFieldIsValidAndCategoryExists()
    {
        // Arrange
        var category = new Category("Tables");
        await CategoryTestHelper.CreateCategory(_appFactory, category);
        var subCategoryName = "Coffee Table";
        var requestPayload = new CreateSubCategoryRequest(subCategoryName);

        // Act
        var response = await _httpClient.PostAsJsonAsync($"api/categories/{category.Id}/subcategories", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var subCategoryResponse = await HttpResponseHelper.ReadFromResponse<SubCategoryResponse>(response);
        subCategoryResponse.Should().NotBeNull();
        subCategoryResponse!.Name.Should().Be(subCategoryName);
        response.Headers.Location!.Should().Be($"http://localhost/api/categories/{category.Id}/SubCategories/{subCategoryResponse!.SubCategoryId}");
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync()
    {
        await SubCategoryTestHelper.ClearAllCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }

}
