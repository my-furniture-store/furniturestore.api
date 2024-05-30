using FurnitureStore.Application.Products.Queries.ListCategoryProducts;
using FurnitureStore.Application.Products.Queries.ListSubCategoryProducts;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Products;
using NSubstitute;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Queries;

public class ListSubCategoryProductsQueryHandlerTests
{
    private readonly ListSubCategoryProductsQueryHandler _sut;
    private readonly IProductsRepository _productsRepository;
    private readonly ISubCategoriesRepository _subCategoriesRepository;
    private readonly Guid _categoryId;
    private readonly Guid _subCategoryId;

    public ListSubCategoryProductsQueryHandlerTests()
    {
        _categoryId = Guid.NewGuid();
        _subCategoryId= SubCategoriesFixture.GetTestSubCategories(_categoryId)[0].Id;
        _subCategoriesRepository = MockSubCategoriesRepository.GetSubCategoriesRepository(_categoryId);
        _productsRepository = MockProductsRepository.GetProductsRepository(_categoryId, _subCategoryId);

        _sut = new(_productsRepository, _subCategoriesRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSubCategoryNotFound_WhenSubCategoryDoesNotExist()
    {
        // Arrange
        var query = new ListSubCategoryProductsQuery(Guid.NewGuid());

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Sub-category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenSubCategoryDoesNotHaveProducts()
    {
        // Arrange
        var subCategoryId = SubCategoriesFixture.GetTestSubCategories(_categoryId)[0].Id;
        _productsRepository.GetProductsBySubCategoryIdAsync(subCategoryId).Returns(Enumerable.Empty<Product>().ToList());
        var query = new ListSubCategoryProductsQuery(subCategoryId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnProductsList_WhenSubCategoryHasProducts()
    {
        // Arrange
        var query = new ListSubCategoryProductsQuery(_subCategoryId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<List<Product>>();
        result.Value.Should().NotBeEmpty();
        result.Value.Count().Should().BeGreaterThan(2);
    }
}
