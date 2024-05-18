using FluentAssertions;
using FurnitureStore.Application.Categories.Commands.CreateCategory;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Categories;
using NSubstitute;

namespace FurnitureStore.Application.Tests.Unit.System.Categories.Commands;

public class CreateCategoryCommandHanlderTests
{
    private readonly CreateCategoryCommandHandler _sut;
    private readonly ICategoriesRepository _categoriesRepository = Substitute.For<ICategoriesRepository>();
    private readonly IUnitofWork _unitofWork = Substitute.For<IUnitofWork>();

    public CreateCategoryCommandHanlderTests()
    {
        _sut = new(_categoriesRepository, _unitofWork);
    }


    [Fact]
    public async Task Handle_ShouldReturnCategory_WhenCategoryIsCreated()
    {
        // Arrange
        var categoryName = "Chairs";
        var command = new CreateCategoryCommand(categoryName);

        // Act
        var result =  await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Should().BeOfType<Category>();
        await _categoriesRepository.Received().AddCategoryAsync(Arg.Is<Category>(c => c.Name == categoryName));
    }

}
