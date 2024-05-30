using FurnitureStore.Application.Products.Queries.ListProducts;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Products;


namespace FurnitureStore.Application.Tests.Unit.System.Products.Queries;

public class ListProductsQueryHandlerTests
{
    private readonly ListProductsQueryHandler _sut;
    private readonly IProductsRepository _productsRepository;
    private readonly Guid _categoryId;
    private readonly Guid _subCategoryId;

    public ListProductsQueryHandlerTests()
    {
        _categoryId = Guid.NewGuid();
        _subCategoryId = Guid.NewGuid();
        _productsRepository = MockProductsRepository.GetProductsRepository(_categoryId, _subCategoryId);

        _sut = new(_productsRepository);
    }


    [Fact]
    public async Task ShouldReturnEmptyList_WhenNoProductsExist()
    {
        // Arrange
        _productsRepository.GetAllAsync().Returns(Enumerable.Empty<Product>().ToList());
        var query = new ListProductsQuery();
        
        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task ShouldReturnProductsList_WhenProductsExist()
    {
        // Arrange
        var query = new ListProductsQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<List<Product>>();
        result.Value.Should().NotBeEmpty();
        result.Value.Count.Should().BeGreaterThan(2);
    }

}
