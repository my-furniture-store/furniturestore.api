using ErrorOr;
using FluentAssertions;
using FurnitureStore.Application.Categories.Queries.GetCategory;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Application.Tests.Unit.Mocks;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Tests.Common.Fixtures;
using NSubstitute;

namespace FurnitureStore.Application.Tests.Unit.System.Categories.Queries;

public class GetCategoryQueryHandlerTests
{
    private readonly GetCategoryQueryHandler _sut;
    private readonly ICategoriesRepository _categoriesRepository = MockCategoriesRepository.GetCategoriesRepository();

    public GetCategoryQueryHandlerTests()
    {
        _sut = new(_categoriesRepository);
    }


    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var category = new Category("Chairs");
        var query = new GetCategoryQuery(category.Id);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors[0].Type.Should().Be(ErrorType.NotFound);
        result.Errors[0].Description.Should().Be("Category not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var category = CategoriesFixture.GetTestCategories()[0];
        var query = new GetCategoryQuery(category.Id);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeOfType<Category>();
        result.Value.Should().BeEquivalentTo(category);
        await _categoriesRepository.Received().ExistsAsync(Arg.Is<Guid>(category.Id));
        await _categoriesRepository.Received().GetByIdAsync(Arg.Is<Guid>(category.Id));
    }

}
