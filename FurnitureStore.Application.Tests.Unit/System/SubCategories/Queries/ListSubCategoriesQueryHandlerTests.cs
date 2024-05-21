using FurnitureStore.Application.SubCategories.Queries.ListSubCategories;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Application.Tests.Unit.System.SubCategories.Queries;

public class ListSubCategoriesQueryHandlerTests
{
    private readonly ListSubCategoriesQueryHandler _sut;
    private readonly Guid _categoryId;
    private readonly ICategoriesRepository _categoriesRepository = MockCategoriesRepository.GetCategoriesRepository();
    private readonly ISubCategoriesRepository _subCategoriesRepository;

    public ListSubCategoriesQueryHandlerTests()
    {
        _categoryId = CategoriesFixture.GetTestCategories()[0].Id;
        _subCategoriesRepository = MockSubCategoriesRepository.GetSubCategoriesRepository(_categoryId);
        _sut = new(_categoriesRepository, _subCategoriesRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoryNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var query = new ListSubCategoriesQuery(CategoryId: Guid.NewGuid());

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Category not found.");
    }


    [Fact]
    public async Task Handle_ShouldReturnListOfSubCategories_WhenCategoryExistsWithAssignedSubCategories()
    {
        // Arrange
        var query = new ListSubCategoriesQuery(_categoryId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<List<SubCategory>>();
        result.Value.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenCategoryExistsWithNoAssignedSubCategories()
    {
        // Arrange
        var categoryId = CategoriesFixture.GetTestCategories()[1].Id;
        var query = new ListSubCategoriesQuery(categoryId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Act
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();

    }
}
