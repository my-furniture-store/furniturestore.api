using FluentAssertions;
using FurnitureStore.Application.Categories.Queries.ListCategories;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Tests.Common.Fixtures;
using NSubstitute;

namespace FurnitureStore.Application.Tests.Unit.System.Categories.Queries;

public class ListCategoriesQueryHandlerTests
{
    private readonly ListCategoriesQueryHandler _sut;
    private readonly ICategoriesRepository _categoriesRepository = Substitute.For<ICategoriesRepository>();

    public ListCategoriesQueryHandlerTests()
    {
        _sut = new(_categoriesRepository);
    }


    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCategoriesExist()
    {
        // Arrange       
        _categoriesRepository.GetAllCategoriesAsync().Returns(Enumerable.Empty<Category>().ToList());
        var query = new ListCategoriesQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }


    [Fact]
    public async Task Handle_ShouldReturnListOfCategories_WhenCategoriesExist()
    {
        // Arrange
        var categoriesCount = CategoriesFixture.GetTestCategories.Count;
        _categoriesRepository.GetAllCategoriesAsync().Returns(CategoriesFixture.GetTestCategories);
        var query = new ListCategoriesQuery();

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeEmpty();
        result.Value.Should().BeOfType<List<Category>>();
        result.Value.Count.Should().Be(categoriesCount);
    }


}
