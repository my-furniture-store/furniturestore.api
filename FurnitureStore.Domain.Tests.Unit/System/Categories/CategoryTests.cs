using ErrorOr;
using FluentAssertions;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.SubCategories;
using FurnitureStore.Domain.Tests.Unit.Fixtures;
using FurnitureStore.Domain.Tests.Unit.Helpers;

namespace FurnitureStore.Domain.Tests.Unit.System.Categories;

public class CategoryTests
{
    private readonly Category _sut;
    private readonly Guid _categoryId = Guid.NewGuid();

    public CategoryTests()
    {
        _sut = new Category("Sofas", _categoryId);

        CategoryTestHelper.SetSubCategories(_sut, SubCategoriesFixture.GetTestSubCategories(_categoryId));
    }


    [Fact]
    public void UpdateCategory_ShouldUpdateCategoryName_WhenNameIsProvided()
    {
        // Arrange
        var categoryName = "Chairs";

        // Act
        _sut.UpdateCategory(categoryName);


        // Assert
        _sut.Name.Should().Be(categoryName);
    }


    [Fact]
    public void AddSubCategory_ShouldAddSubCategory_WhenSubCategoryDoesNotExist()
    {
        // Arrange
        var subCategory = new SubCategory("Cabriole", _categoryId, Guid.NewGuid());

        // Act
        var result = _sut.AddSubCategory(subCategory);

        // Assert
        result.IsError.Should().BeFalse();
    }


    [Fact]
    public void AddSubCategory_ShouldReturnError_WhenCategoryAlreadyExists()
    {
        // Arrange
        var subCategory = SubCategoriesFixture.GetTestSubCategories(_categoryId)[0];

        // Act
        var result = _sut.AddSubCategory(subCategory);

        // Assert
        result.FirstError.Description.Should().Be("A category cannot have duplicate sub-categories.");
        result.Errors.Should().NotBeEmpty();
    }


    [Fact]
    public void RemoveSubCategory_ShouldThrowException_WhenSubCategoryIsNotPresent()
    {
        // Arrange
        var subCategory = new SubCategory("Lawson", _categoryId);

        // Act
        Action act = () => _sut.RemoveSubCategory(subCategory);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage($"Sub-category, {subCategory.Name}, does not exist in category.");
    }

    [Fact]
    public void RemoveSubCategory_ShouldRemoveSubCategory_WhenSubCategoryIsPresent()
    {
        // Arrange
        var subCategory = SubCategoriesFixture.GetTestSubCategories(_categoryId)[1];

        // Act
        _sut.RemoveSubCategory(subCategory);

        // Assert
        _sut.SubCategories.Should().NotContain(subCategory);
    }

    [Fact]
    public void HasSubCategory_ShouldReturnTrue_WhenSubCategoryIsPresent()
    {
        // Arrange
        var subCategory = SubCategoriesFixture.GetTestSubCategories(_categoryId)[1];

        // Act
        var result = _sut.HasSubCategory(subCategory.Id);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void HasSubCategory_ShouldReturnFalse_WhenSubCategoryIsNotPresent()
    {
        // Arrange
        var subCategory = new SubCategory("Lawson", Guid.NewGuid());

        // Act
        var result = _sut.HasSubCategory(subCategory.Id);

        // Assert
        result.Should().BeFalse();
    }



}
