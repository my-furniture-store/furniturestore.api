using FurnitureStore.Application.SubCategories.Commands.UpdateSubCategory;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Application.Tests.Unit.System.SubCategories.Commands;

public class UpdateSubCategoryCommandHanlderTests
{
    private readonly Guid _categoryId;
    private readonly UpdateSubCategoryCommandHandler _sut;
    private readonly ISubCategoriesRepository _subCategoriesRepository;
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();

    public UpdateSubCategoryCommandHanlderTests()
    {
        _categoryId = Guid.NewGuid();
        _subCategoriesRepository = MockSubCategoriesRepository.GetSubCategoriesRepository(_categoryId);
        _sut = new(_subCategoriesRepository, _unitofWork);
    }

    [Fact]
    public async Task Handle_ShouldUpdateSubCategory_WhenSubCategoryExists()
    {
        // Arrange
        var newName = "Loveseats";
        var subCategoryId = SubCategoriesFixture.GetTestSubCategories(_categoryId)[0].Id;
        var command = new UpdateSubCategoryCommand(CategoryId: _categoryId, SubCategoryId: subCategoryId, newName);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<SubCategory>();
        result.Value.Name.Should().Be(newName);
        await _subCategoriesRepository.Received().UpdateAsync(Arg.Is<SubCategory>(x => x.Id == subCategoryId));
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenSubCategoryDoesNotExist()
    {
        // Arrange
        var command = new UpdateSubCategoryCommand(_categoryId, SubCategoryId: Guid.NewGuid(), "Loveseats");

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Sub-category not found.");
    }

}
