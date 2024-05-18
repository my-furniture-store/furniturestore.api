using FluentAssertions;
using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Domain.Tests.Unit.System.SubCategories;

public class SubCategoryTests
{
    private readonly SubCategory _sut;

    public SubCategoryTests()
    {
        _sut = new SubCategory("Lawson", Guid.NewGuid());
    }

    [Fact]
    public void UpdateSubCategory_ShouldUpdateCategoryName_WhenNameIsProvided()
    {
        // Arrange
        var subCategoryName = "Chesterfield";

        // Act
        _sut.UpdateSubCategory(subCategoryName);


        // Assert
        _sut.Name.Should().Be(subCategoryName);
    }
}
