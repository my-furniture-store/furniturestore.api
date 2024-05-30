using FurnitureStore.Application.Products.Queries.GetProduct;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Queries;

public class GetProductQueryHandlerTest
{
    private readonly GetProductQueryHandler _sut;
    private readonly IProductsRepository _productsRepository;
    private readonly Guid _categoryId;
    private readonly Guid _subCategoryId;
    public GetProductQueryHandlerTest()
    {
        _categoryId = Guid.NewGuid();
        _subCategoryId = Guid.NewGuid();
        _productsRepository = MockProductsRepository.GetProductsRepository(_categoryId, _subCategoryId);

        _sut = new(_productsRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var query = new GetProductQuery(Guid.NewGuid());

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Product not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = ProductsFixture.GetListofProducts(_categoryId, _subCategoryId)[0];
        var query = new GetProductQuery(product.Id);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<Product>();
        result.Value.Should().BeEquivalentTo(product, options => options.Excluding(product => product.DateAdded));

    }
}
