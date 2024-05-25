using FurnitureStore.Application.Products.Commands.DeleteProduct;
using FurnitureStore.Application.Products.Commands.SetProductDescription;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Commands;

public class SetProductDescriptionCommandHandlerTests
{
    private readonly SetProductDescriptionCommandHandler _sut;
    private readonly IProductsRepository _productsRepository;
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();
    private readonly Guid _categoryId = Guid.NewGuid();
    private readonly Guid _subCategoryId = Guid.NewGuid();

    public SetProductDescriptionCommandHandlerTests()
    {
        _productsRepository = MockProductsRepository.GetProductsRepository(_categoryId, _subCategoryId);

        _sut = new(_productsRepository, _unitofWork);
    }


    [Fact]
    public async Task Handle_ShouldReturnProductNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new SetProductDescriptionCommand(Guid.NewGuid(), "Some description...");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Product not found.");
    }

    [Fact]
    public async Task Hanld_ShouldSetProductDescription_WhenProductExists()
    {
        // Arrange
        var description = "Some description...";
        var productId = ProductsFixture.GetListofProducts(_categoryId, _subCategoryId)[0].Id;
        var command = new SetProductDescriptionCommand(productId, description);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<Product>();
        result.Value.Description.Should().Be(description);
        await _productsRepository.Received().UpdateAsync(Arg.Is<Product>(p => p.Description == description));

    }
}
