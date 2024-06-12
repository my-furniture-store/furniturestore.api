using FurnitureStore.Application.Products.Queries.ListFeaturedProducts;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Queries;

public class ListFeaturedProductsQueryHandlerTests
{
    private readonly ListFeaturedProductsQueryHandler _sut;
    private readonly IProductsRepository _productsRepository;
    private readonly Guid _categoryId;
    private readonly Guid _subCategoryId;

    public ListFeaturedProductsQueryHandlerTests()
    {
        _categoryId = Guid.NewGuid();
        _subCategoryId = Guid.NewGuid();
        _productsRepository = MockProductsRepository.GetProductsRepository(_categoryId, _subCategoryId);

        _sut = new(_productsRepository);
    }


    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoFeaturedProductsExist()
    {
        // Arrange
        _productsRepository.GetFeaturedProductAsync().Returns(Enumerable.Empty<Product>().ToList());
        var query = new ListFeaturedProductsQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }


    [Fact]
    public async Task Handle_ShouldReturnProductsList_WhenFeaturedProductsExists()
    {
        // Arrange
        var query = new ListFeaturedProductsQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<List<Product>>();
        result.Value.Count.Should().Be(2);
    }
}
