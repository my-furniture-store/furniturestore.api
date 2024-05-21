using FurnitureStore.Application.SubCategories.Queries.GetSubCategory;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Application.Tests.Unit.System.SubCategories.Queries;

public class GetSubCategoryQueryHandlerTests
{
    private readonly GetSubCategoryQueryHandler _sut;
    private readonly Guid _categoryId;
    private readonly ICategoriesRepository _categoriesRepository = MockCategoriesRepository.GetCategoriesRepository();
    private readonly ISubCategoriesRepository _subCategoriesRepository;

    public GetSubCategoryQueryHandlerTests()
    {
        _categoryId = CategoriesFixture.GetTestCategories()[0].Id;
        _subCategoriesRepository = MockSubCategoriesRepository.GetSubCategoriesRepository(_categoryId);
        _sut = new(_categoriesRepository, _subCategoriesRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoryNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var query = new GetSubCategoryQuery(SubCategoryId: Guid.NewGuid(), CategoryId:  Guid.NewGuid());

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSubCategoryNotFound_WhenSubCategoryDoesNotExist()
    {
        // Arrange
        var query = new GetSubCategoryQuery(SubCategoryId: Guid.NewGuid(), CategoryId: _categoryId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Sub-category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSubCategory_WhenSubCategoryExists()
    {
        // Arrange
        var subCategory = SubCategoriesFixture.GetTestSubCategories(_categoryId)[0];
        var query = new GetSubCategoryQuery(subCategory.Id, _categoryId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<SubCategory>();
        result.Value.Should().BeEquivalentTo(subCategory);
    }
}
