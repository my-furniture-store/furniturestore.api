using FurnitureStore.Contracts.Categories;
using FurnitureStore.Domain.Categories;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.CategoriesController;

[Collection("FurnitureStore.API Collection")]
public class GetCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;
    private readonly Faker<Category> _categoryGenerator = new Faker<Category>()
        .RuleFor(c => c.Id, faker => Guid.NewGuid())
        .RuleFor(c => c.Name, faker => faker.Commerce.Categories(1)[0]);

    public GetCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async Task Get_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync($"api/categories/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);

        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
    }


    [Fact]
    public async Task Get_ReturnsCategory_WhenCategoryExists()
    {
        // Arrange
        var category = new Category(name:"Chairs",id: Guid.NewGuid());
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        // Act
        var response = await _httpClient.GetAsync($"api/categories/{category.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categoryResponse = await HttpResponseHelper.ReadFromResponse<CategoryResponse>(response);
        categoryResponse!.Should().BeEquivalentTo(category);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
}
