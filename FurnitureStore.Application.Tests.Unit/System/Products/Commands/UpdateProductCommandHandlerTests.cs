using FurnitureStore.Application.Products.Commands.UpdateProduct;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Commands;

public class UpdateProductCommandHandlerTests 
{
    private readonly UpdateProductCommandHandler _sut;
    private readonly IProductsRepository _productsRepository;
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();
    private readonly Guid _categoryId;
    private readonly Guid _subCategoryId;

    public UpdateProductCommandHandlerTests()
    {
        _categoryId = CategoriesFixture.GetTestCategories()[0].Id;
        _subCategoryId = SubCategoriesFixture.GetTestSubCategories(_categoryId)[0].Id;

        _productsRepository = MockProductsRepository.GetProductsRepository(_categoryId, _subCategoryId);

        _sut = new(_productsRepository, _unitofWork);
    }


    [Fact]
    public async Task Handle_ShouldReturnProductNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new UpdateProductCommand(Guid.NewGuid(), "TestName", 10.00m);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Product not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnDuplicateColorsError_WhenUpdatingProductWithDuplicatColors()
    {
        // Arrange
        var colors = new List<ProductColor> { new ProductColor { Name = "Light Red", Code = "#FF0000" } };
        var productId = ProductsFixture.GetListofProducts(_categoryId, _subCategoryId)[0].Id;
        var command = new UpdateProductCommand(ProductId:  productId, Colors:  colors);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Should().Be(ProductErrors.CannotHaveDuplicateColors);
    }

    [Fact]
    public async Task Handle_ShouldReturnUpdatedProduct_WhenProductExists()
    {
        // Arrange
        var productId = ProductsFixture.GetListofProducts(_categoryId, _subCategoryId)[0].Id;
        var newName = "New Product Name";
        var newPrice = 15.99m;
        var command = new UpdateProductCommand(productId, newName, newPrice);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<Product>();
        result.Value.Name.Should().Be(newName);
        result.Value.Price.Should().Be(newPrice);
        await _productsRepository.Received().UpdateAsync(Arg.Is<Product>(p => p.Name == newName));
    }

}
