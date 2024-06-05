using FurnitureStore.Contracts.Products;
using FurnitureStore.Tests.Common.Fixtures;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.ProductsController;

[Collection("FurnitureStore.API Collection")]
public class CreateProductsControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public CreateProductsControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async void Create_ReturnValidationError_WhenPriceIsMissingOrLessThan25()
    {
        // Arrange
        var requestPayload = new CreateProductRequest("Product1", Price: 0, Guid.NewGuid(), Guid.NewGuid());

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/products", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationError = await HttpResponseHelper.GetFromResponse<ValidationProblemDetails>(response);
        validationError!.Status.Should().Be(400);
        validationError.Title.Should().Be("One or more validation errors occurred.");
        validationError.Errors["Price"][0].Should().Be("The field Price must be greater than or equal to 25.");
    }

    [Fact]
    public async Task Create_ReturnProblem_WhenCategoryIsNotSpecified()
    {
       // Arrange
        var requestPayload = new CreateProductRequest("Product1", Price: 27.0m, Guid.Empty, Guid.NewGuid());

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/products", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await HttpResponseHelper.GetFromResponse<ProblemDetails>(response);
        problem!.Status.Should().Be(400);
        problem.Title.Should().Be("Bad Request");
        problem.Detail.Should().Be("Category is required.");
    }

    [Fact]
    public async Task Create_ReturnsProblem_WhenSubCategoryIsNotSpecified()
    {
        // Arrange

        var requestPayload = new CreateProductRequest("Product1", 26m, Guid.NewGuid(), Guid.Empty);

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/products", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await HttpResponseHelper.GetFromResponse<ProblemDetails>(response);
        problem!.Status.Should().Be(400);
        problem.Title.Should().Be("Bad Request");
        problem.Detail.Should().Be("Sub-category is required.");
    }

    [Fact]
    public async Task Create_ReturnsNotFound_WhenCategorySpecifiedDoesNotExist()
    {
        // Arrange
        var requestPayload = new CreateProductRequest("Product1", 26m, Guid.NewGuid(), Guid.NewGuid());

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/products", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.GetFromResponse<ProblemDetails>(response);
        problem!.Status.Should().Be(404);
        problem.Title.Should().Be("Not Found");
        problem.Detail.Should().Be("Category not found.");
    }


    [Fact]
    public async Task Create_ReturnsNotFound_ForMissingSubCategoryInSpecifiedCategory()
    {
        // Arrange
        var category = CategoriesFixture.GetTestCategories()[1];
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        var requestPayload = new CreateProductRequest("Product1", 26m, category.Id, Guid.NewGuid());

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/products", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.GetFromResponse<ProblemDetails>(response);
        problem!.Status.Should().Be(404);
        problem.Title.Should().Be("Not Found");
        problem.Detail.Should().Be("Sub-category not found.");
    }


    [Fact]
    public async Task Create_ReturnsCreatedProduct_WhenProductIsCreated()
    {
        //Arrange
        var category = CategoriesFixture.GetTestCategories()[1];
        var subCategory = SubCategoriesFixture.GetTestSubCategories(category.Id)[2];
       
        await CategoryTestHelper.CreateCategory(_appFactory, category);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);

        var requestPayload = new CreateProductRequest("Product1", 26m, category.Id, subCategory.Id);

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/products", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var productResponse = await HttpResponseHelper.ReadFromResponse<ProductResponse>(response);

        response.Headers.Location!.Should().Be($"http://localhost/api/Products/{productResponse!.Id}");
        productResponse.Should().NotBeNull();
        productResponse.Should().BeOfType<ProductResponse>();
        productResponse!.Name.Should().Be("Product1");
        productResponse!.Price.Should().Be(26m);
    }


    public async Task InitializeAsync()
    {
        await ProductTestHelper.ClearAllProducts(_appFactory);
        await SubCategoryTestHelper.ClearAllSubCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }

    public async Task DisposeAsync()
    {
        await ProductTestHelper.ClearAllProducts(_appFactory);
        await SubCategoryTestHelper.ClearAllSubCategories(_appFactory);
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
     
}
