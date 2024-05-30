using FurnitureStore.Application.Products.Queries.ListCategoryProducts;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Queries;

public class ListCategoryProductsQueryHandlerTests 
{
    private readonly ListCategoryProductsQueryHandler _sut;
    private readonly IProductsRepository _productsRepository;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly Guid _categoryId;
    private readonly Guid _subCategoryId;


    public ListCategoryProductsQueryHandlerTests()
    {
        _categoryId = CategoriesFixture.GetTestCategories()[0].Id;
        _subCategoryId = Guid.NewGuid();
        _categoriesRepository = MockCategoriesRepository.GetCategoriesRepository();
        _productsRepository = MockProductsRepository.GetProductsRepository(_categoryId, _subCategoryId);

        _sut = new(_productsRepository, _categoriesRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoryNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var query = new ListCategoryProductsQuery(Guid.NewGuid());

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenCategoryDoesNotHaveProducts()
    {
        // Arrange
        var categoryId = CategoriesFixture.GetTestCategories()[0].Id;
        _productsRepository.GetProductsByCategoryIdAsync(categoryId).Returns(Enumerable.Empty<Product>().ToList());
        var query = new ListCategoryProductsQuery(categoryId);

        // Act
        var result = await  _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnProductsList_WhenCategoryHasProducts()
    {
        // Arrange
        var query = new ListCategoryProductsQuery(_categoryId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<List<Product>>();
        result.Value.Should().NotBeEmpty();
        result.Value.Count().Should().BeGreaterThan(2);
    }

}
