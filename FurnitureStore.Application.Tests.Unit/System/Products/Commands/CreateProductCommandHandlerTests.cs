using FurnitureStore.Application.Products.Commands.CreateProduct;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Commands;

public class CreateProductCommandHandlerTests
{
    private readonly Guid _categoryId;
    private readonly Guid _subCategoryId;
    private readonly ICategoriesRepository _categoriesRepository = MockCategoriesRepository.GetCategoriesRepository();
    private readonly ISubCategoriesRepository _subCategoriesRepository;
    private readonly IProductsRepository _productsRepository;
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();

    private readonly CreateProductCommandHandler _sut;

    public CreateProductCommandHandlerTests()
    {
        _categoryId = CategoriesFixture.GetTestCategories()[0].Id;
        _subCategoryId = SubCategoriesFixture.GetTestSubCategories(_categoryId)[0].Id;
        _subCategoriesRepository = MockSubCategoriesRepository.GetSubCategoriesRepository(_categoryId);
        _productsRepository = MockProductsRepository.GetProductsRepository(_categoryId, _subCategoryId);

        _sut = new(_productsRepository, _unitofWork, _categoriesRepository, _subCategoriesRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoryNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var command = new CreateProductCommand("Test", 10m, Guid.NewGuid(), _subCategoryId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().NotBeNull();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSubCategoryNotFound_WhenSubCategoryDoesNotExist()
    {
        // Arrange
        var command = new CreateProductCommand("Test", 10m, _categoryId, Guid.NewGuid());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().NotBeNull();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Sub-category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSubCategoryNotFound_WhenSubCategoryIsNotInCategory()
    {
        // Arrange
        var categoryId = CategoriesFixture.GetTestCategories()[1].Id;
        var command = new CreateProductCommand("Test", 10m, categoryId, _subCategoryId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().NotBeNull();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Sub-category not found.");
    }

    [Fact]
    public async Task Handle_ShouldAddProduct_WhenDataIsCorrect()
    {
        // Arrange
        var productName = "Product Z";
        var command = new CreateProductCommand(productName, 10m, _categoryId, _subCategoryId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<Product>();
        result.Value.Name.Should().Be(productName);
        await _productsRepository.Received().AddAsync(Arg.Is<Product>(product => product.Name == productName));

    }
}
