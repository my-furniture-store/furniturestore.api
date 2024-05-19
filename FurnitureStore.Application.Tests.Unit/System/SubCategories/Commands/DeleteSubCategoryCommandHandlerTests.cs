using FurnitureStore.Application.SubCategories.Commands.DeleteSubCategory;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Application.Tests.Unit.System.SubCategories.Commands;

public class DeleteSubCategoryCommandHandlerTests
{
    private readonly Guid _categoryId;
    private readonly DeleteSubCategoryCommandHandler _sut;
    private readonly ICategoriesRepository _categoriesRepository = MockCategoriesRepository.GetCategoriesRepository();
    private readonly ISubCategoriesRepository _subCategoriesRepository;
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();

    public DeleteSubCategoryCommandHandlerTests()
    {
        _categoryId = CategoriesFixture.GetTestCategories()[0].Id;
        _subCategoriesRepository = MockSubCategoriesRepository.GetSubCategoriesRepository(_categoryId);
        _sut = new(_categoriesRepository, _subCategoriesRepository, _unitofWork);
    }

    [Fact]
    public async Task Handle_ShouldReturnSubCategoryNotFound_WhenSubCategoryDoesNotExist()
    {
        // Arrange
        var subCategory = new SubCategory("Loveseats", categoryId: _categoryId);
        var command = new DeleteSubCategoryCommand(CategoryId: _categoryId, SubCategoryId: subCategory.Id);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Sub-category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoryNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var category = new Category("Beds");
        var subCategory = SubCategoriesFixture.GetTestSubCategories(_categoryId)[0];
        var command = new DeleteSubCategoryCommand(CategoryId:category.Id, SubCategoryId:subCategory.Id);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSubCategoryNotFound_WhenSubCategoryDoesNotExistInCategorySpecified()
    {
        // Arrange
        var category = CategoriesFixture.GetTestCategories()[1];
        var subCategory = SubCategoriesFixture.GetTestSubCategories(_categoryId)[0];

        var command = new DeleteSubCategoryCommand(CategoryId: category.Id, SubCategoryId:subCategory.Id);

        // Act 
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Sub-category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnDeleted_WhenSubCategoryIsRemovedFromSpecifiedCategory()
    {
        // Arrange
        var subCategory = SubCategoriesFixture.GetTestSubCategories(_categoryId)[0];
        var command = new DeleteSubCategoryCommand(CategoryId: _categoryId, SubCategoryId:subCategory.Id);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<Deleted>();
        await _subCategoriesRepository.Received().RemoveSubCategoryAsync(Arg.Is<SubCategory>(sc => sc.Id == subCategory.Id));
    }
}
