
using FurnitureStore.Contracts.Categories;
using FurnitureStore.Domain.Categories;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.CategoriesController;

[Collection("FurnitureStore.API Collection")]
public class CreateCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;
    
    public CreateCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async void Create_ReturnsCreatedCategory_WhenCategoryIsCreated()
    {
        // Arrange
        var category = new CreateCategoryRequest(Name: "Chairs");

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/categories", category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var categoryResponse = await HttpResponseHelper.ReadFromResponse<CategoryResponse>(response);
        categoryResponse.Should().BeEquivalentTo(category);
        response.Headers.Location!.Should().Be($"http://localhost/api/Categories/{categoryResponse!.Id}");
    }


    [Fact]
    public async Task Create_ReturnsValidationError_WhenCategoryNameIsEmpty()
    {
        // Arrange
        var category = new CreateCategoryRequest(Name: "");

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/categories", category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        error!.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Name"][0].Should().Be("The Name field is required.");
    }

    [Fact]
    public async Task Create_ReturnsValidationError_WhenCategoryNameIs50OrMoreCharacters()
    {
        // Arrange
        var category = new Category("Chairs");
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        var invalidName = new String('A', 51);
        var categoryUpdateRequest = new CreateCategoryRequest(Name: invalidName);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/categories/{category.Id}", categoryUpdateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);
        error!.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Name"][0].Should().Be("The field Name must be a string with a maximum length of 50.");
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync()
    {
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }

}
