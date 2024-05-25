using FurnitureStore.Application.Products.Commands.DeleteProduct;
using FurnitureStore.Application.Products.Commands.UpdateProduct;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Products;
namespace FurnitureStore.Application.Tests.Unit.System.Products.Commands;

public class DeleteProductCommandHandlerTests
{
    private readonly DeleteProductCommandHandler _sut;
    private readonly IProductsRepository _productsRepository;
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();
    private readonly Guid _categoryId = Guid.NewGuid();
    private readonly Guid _subCategoryId = Guid.NewGuid();

    public DeleteProductCommandHandlerTests()
    {
        _productsRepository = MockProductsRepository.GetProductsRepository(_categoryId, _subCategoryId);

        _sut = new(_productsRepository, _unitofWork);
    }

    [Fact]
    public async Task Handle_ShouldReturnProductNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new DeleteProductCommand(Guid.NewGuid());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Product not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnDelete_WhenProductExists()
    {
        // Arrange
        var productId = ProductsFixture.GetListofProducts(_categoryId, _subCategoryId)[0].Id;
        var command = new DeleteProductCommand(productId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<Deleted>();
        await _productsRepository.Received().RemoveProductAsync(Arg.Is<Product>(p => p.Id == productId));
    }
}
