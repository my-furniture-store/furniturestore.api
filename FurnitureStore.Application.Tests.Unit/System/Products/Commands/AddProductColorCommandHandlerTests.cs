using FurnitureStore.Application.Products.Commands.AddProductColor;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Commands;

public class AddProductColorCommandHandlerTests
{
    private readonly AddProductColorCommandHandler _sut;
    private readonly IProductsRepository _productsRepository;
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();
    private readonly Guid _categoryId = Guid.NewGuid();
    private readonly Guid _subCategoryId = Guid.NewGuid();

    public AddProductColorCommandHandlerTests()
    {
        _productsRepository = MockProductsRepository.GetProductsRepository(_categoryId, _subCategoryId);

        _sut = new(_productsRepository, _unitofWork);
    }

    [Fact]
    public async Task Handle_ShouldReturnProductNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new AddProductColorCommand(Guid.NewGuid(), "Red", "#ff0000");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Product not found.");
    }


    [Fact]
    public async Task Handle_ShouldReturnError_WhenAddingDuplicateErrors()
    {
        // Arrange
        var product = ProductsFixture.GetListofProducts(_categoryId, _subCategoryId)[0];
        var command = new AddProductColorCommand(product.Id, "Red-white", "#ff0000");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Should().Be(ProductErrors.CannotHaveDuplicateColors);
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenColorIsAdded()
    {
        // Arrange
        var product = ProductsFixture.GetListofProducts(_categoryId, _subCategoryId)[0];
        var colorName = "Grey";
        var colorCode = "#FCFCFC";
        var command = new AddProductColorCommand(product.Id, colorName, colorCode);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<Product>();

        var productResult = await _productsRepository.GetByIdAsync(product.Id);

        productResult.Should().NotBeNull();
        productResult!.Colors.Should().Contain(productColor => productColor.Name == colorName && productColor.Code == colorCode);
    }

}
