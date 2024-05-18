using FluentAssertions;
using FurnitureStore.Application.Categories.Commands.DeleteCategory;
using FurnitureStore.Application.Categories.Commands.UpdateCategory;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Tests.Common.Fixtures;
using NSubstitute;

namespace FurnitureStore.Application.Tests.Unit.System.Categories.Commands;

public class UpdateCategoryCommandHandlerTests
{
    private readonly UpdateCategoryCommandHandler _sut;
    private readonly ICategoriesRepository _categoriesRepository = MockCategoriesRepository.GetCategoriesRepository();
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();

    public UpdateCategoryCommandHandlerTests()
    {
        _sut = new(_categoriesRepository, _unitofWork);
    }


    [Fact]
    public async Task Handle_ShouldUpdateCategory_WhenCategoryExists()
    {
        // Arrange
        var newName = "Beds";
        var category = CategoriesFixture.GetTestCategories[0];
        var updateCommand = new UpdateCategoryCommand(category.Id, newName);

        // Act
        var result = await _sut.Handle(updateCommand, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<Category>();
        result.Value.Name.Should().Be(newName);
        await _categoriesRepository.Received().UpdateCategoryAsync(Arg.Is<Category>(c => c.Name == newName));
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var updateCommand = new UpdateCategoryCommand(Guid.NewGuid(), "Beds");

        // Act
        var result = await _sut.Handle(updateCommand, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Description.Should().Be("Category not found.");
        await _categoriesRepository.DidNotReceive().UpdateCategoryAsync(Arg.Any<Category>());
    }

}
