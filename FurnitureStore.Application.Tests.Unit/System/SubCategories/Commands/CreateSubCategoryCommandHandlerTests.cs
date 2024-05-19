using FurnitureStore.Application.SubCategories.Commands.CreateSubCategory;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Application.Tests.Unit.System.SubCategories.Commands;

public class CreateSubCategoryCommandHandlerTests
{
    private readonly Guid _categoryId;
    private readonly CreateSubCategoryCommandHandler _sut;
    private readonly ICategoriesRepository _categoriesRepository = MockCategoriesRepository.GetCategoriesRepository();
    private readonly ISubCategoriesRepository _subCategoriesRepository;
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();

    public CreateSubCategoryCommandHandlerTests()
    {
        _categoryId = CategoriesFixture.GetTestCategories()[0].Id;
        _subCategoriesRepository = MockSubCategoriesRepository.GetSubCategoriesRepository(_categoryId);
        _sut = new(_categoriesRepository, _subCategoriesRepository, _unitofWork);
    }


    [Fact]
    public async Task Handle_ShouldAddSubCategoryToCategory_WhenCategoryExists()
    {
        // Arrange
        var subCategoryName = "Lawson Sofas";
        var command = new CreateSubCategoryCommand(subCategoryName, _categoryId);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<SubCategory>();
        result.Value.Name.Should().Be(subCategoryName);
        result.Value.CategoryId.Should().Be(_categoryId);
        await _subCategoriesRepository.Received().AddSubCategoryAsync(Arg.Is<SubCategory>(sc => sc.CategoryId == _categoryId));
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoryNotFound_WhenCategoryDoesNotExist()
    {
        //Arrange
        var command = new CreateSubCategoryCommand("Lawson Sofas", Guid.NewGuid());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Should().NotBeNull();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Category not found.");        
    }

}
