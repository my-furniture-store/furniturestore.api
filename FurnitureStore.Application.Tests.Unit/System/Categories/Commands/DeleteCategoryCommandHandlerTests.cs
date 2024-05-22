using FurnitureStore.Application.Categories.Commands.DeleteCategory;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Categories;

namespace FurnitureStore.Application.Tests.Unit.System.Categories.Commands;

public class DeleteCategoryCommandHandlerTests
{
    private readonly DeleteCategoryCommandHandler _sut;
    private readonly ICategoriesRepository _categoriesRepository = MockCategoriesRepository.GetCategoriesRepository();
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();

    public DeleteCategoryCommandHandlerTests()
    {
        _sut = new(_categoriesRepository, _unitofWork);
    }


    [Fact]
    public async Task Handle_ShouldDeleteCategory_WhenCategoryExists()
    {
        // Arrange
        var category = CategoriesFixture.GetTestCategories()[1];
        var deleteCommand = new DeleteCategoryCommand(category.Id);

        // Act
        var result = await _sut.Handle(deleteCommand, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<Deleted>();
        await _categoriesRepository.Received().RemoveCategoryAsync(Arg.Is<Category>(c => c.Id == category.Id));
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var category = new Category("Beds");
        var deleteCommand = new DeleteCategoryCommand(category.Id);

        // Act
        var result = await _sut.Handle(deleteCommand, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Description.Should().Be("Category not found.");
        await _categoriesRepository.DidNotReceive().RemoveCategoryAsync(Arg.Any<Category>());
    }

    [Fact]
    public async Task Handle_ShouldReturnConflict_WhenCategoryHasAssociatedSubCategories()
    {
        // Arrange
        var categoryId = CategoriesFixture.GetTestCategories()[0].Id;
        var command = new DeleteCategoryCommand(categoryId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.Conflict);
        result.Errors[0].Description.Should().Be("Can't delete category with associated sub-categories.");
    }

}
