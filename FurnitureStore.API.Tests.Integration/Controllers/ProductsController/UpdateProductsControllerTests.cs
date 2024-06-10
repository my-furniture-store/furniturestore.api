
using FurnitureStore.Contracts.Products;
using FurnitureStore.Domain.Products;
using FurnitureStore.Tests.Common.Fixtures;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.ProductsController;

[Collection("FurnitureStore.API Collection")]
public class UpdateProductsControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;

    public UpdateProductsControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task UpdateProduct_ReturnsProductNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var requestPayload = new UpdateProductRequest();

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/products/{Guid.NewGuid()}", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await HttpResponseHelper.GetFromResponse<ProblemDetails>(response);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(404);
        problemDetails!.Detail.Should().Be("Product not found.");
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnUpdateProduct_WhenProductExists()
    {
        // Arrange
        var product = await CreateProduct();

        var requestPayload = new UpdateProductRequest(
            Name: "Modern Product",
            Price: 399.99m,
            SKU: "PRO1234",
            Brand: "Luxury",
            Material: "Wood and Leather",
            Colors: new List<Contracts.Products.ProductColor>
            {
                new Contracts.Products.ProductColor(ColorCode:"#FCFCFC", ColorName: "Grey")
            },
            Rating: 4.2,
            Discount: 0.025m);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/products/{product.Id}", requestPayload);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var productResponse = await HttpResponseHelper.ReadFromResponse<ProductResponse>(response);

        productResponse.Should().NotBeNull();
        productResponse!.Name.Should().Be(requestPayload.Name);
        productResponse!.Price.Should().Be(requestPayload.Price);
        productResponse!.SKU.Should().Be(requestPayload.SKU);
        productResponse!.Brand.Should().Be(requestPayload.Brand);
        productResponse!.Material.Should().Be(requestPayload.Material);
        productResponse!.Discount.Should().Be(requestPayload.Discount);
        productResponse!.Rating.Should().Be(requestPayload.Rating);
        productResponse!.Colors.Should().ContainEquivalentOf(requestPayload.Colors![0]);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnValidationError_WhenPriceIsLessThan25()
    {
        // Arrange
        var product = await CreateProduct();

        var requestPayload = new UpdateProductRequest(Price: 24.99m);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/products/{product.Id}", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var validationError = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);

        validationError.Should().NotBeNull();
        validationError!.Status.Should().Be(400);
        validationError!.Title.Should().Be("One or more validation errors occurred.");
        validationError!.Errors.Should().HaveCount(1);
        validationError!.Errors["Price"][0].Should().Be("The field Price must be greater than or equal to 25.");
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnBadRequest_WhenAttemptingToDuplicateColors()
    {
        // Arrange
        var product = await CreateProduct();

        var existingColor = product.Colors[0];
        var requestPayload = new UpdateProductRequest(
            Colors: new List<Contracts.Products.ProductColor>
            {
                new Contracts.Products.ProductColor(ColorCode: existingColor.Code, ColorName:existingColor.Name)
            });

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/products/{product.Id}", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await HttpResponseHelper.GetFromResponse<ProblemDetails>(response);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        problemDetails!.Title.Should().Be("Bad Request");
        problemDetails!.Detail.Should().Be("A product cannot have duplicate colors.");
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnBadRequest_WhenProductColorCodeIsInvalid()
    {
        // Arrange
        var product = await CreateProduct();

        var color = new Contracts.Products.ProductColor(ColorCode: "FCFCFC", ColorName: "Gray");
        var requestPayload = new UpdateProductRequest(Colors: new List<Contracts.Products.ProductColor> { color});

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/products/{product.Id}", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await HttpResponseHelper.GetFromResponse<ProblemDetails>(response);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        problemDetails!.Title.Should().Be("Bad Request");
        problemDetails!.Detail.Should().Be("Color code must be in the format #FFFFFF or #ffffff.");
    }
    
    [Fact]
    public async Task UpdateProduct_ShouldReturnBadRequest_WhenProductColorNameIsNotProvided()
    {
        // Arrange
        var product = await CreateProduct();

        var color = new Contracts.Products.ProductColor(ColorCode: "#FCFCFC", ColorName: "");
        var requestPayload = new UpdateProductRequest(Colors: new List<Contracts.Products.ProductColor> { color});

        // Act
        var response = await _httpClient.PutAsJsonAsync($"api/products/{product.Id}", requestPayload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await HttpResponseHelper.GetFromResponse<ProblemDetails>(response);

        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        problemDetails!.Title.Should().Be("Bad Request");
        problemDetails!.Detail.Should().Be("Specify a color name.");
    }


    private async Task<Product> CreateProduct()
    {
        var category = CategoriesFixture.GetTestCategories()[1];
        var subCategory = SubCategoriesFixture.GetTestSubCategories(category.Id)[1];
        var product = ProductsFixture.GetListofProducts(category.Id, subCategory.Id)[2];

        await CategoryTestHelper.CreateCategory(_appFactory, category);
        await SubCategoryTestHelper.CreateSubCategory(_appFactory, subCategory);
        await ProductTestHelper.CreateProduct(_appFactory, product);

        return product;
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public async Task DisposeAsync()
    {
        await DbContextHelper.ClearAllTables(_appFactory);
    }
}
