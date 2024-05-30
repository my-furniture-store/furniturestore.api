using FluentValidation.TestHelper;
using FurnitureStore.Application.Products.Commands.CreateProduct;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Validators;

public class CreateProductCommandValidatorTests
{
    public readonly CreateProductCommandValidator _sut;

    public CreateProductCommandValidatorTests()
    {
        _sut = new CreateProductCommandValidator();
    }


    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateProductCommand(Name: string.Empty, Price: 10m, Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Product name is required.");
    }


    [Fact]
    public void ShouldHaveError_WhenPriceIsZeroOrLess25()
    {
        // Arrange
        var command = new CreateProductCommand(Name: "Product1", Price: 0, Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Price)
            .WithErrorMessage("Product price must be at least 25.");
    }


    [Fact]
    public void ShouldHaveError_WhenCategoryIdIsEmpty()
    {
        // Arrange
        var command = new CreateProductCommand(Name: "Product1", Price: 26, CategoryId: Guid.Empty, Guid.NewGuid());

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.CategoryId)
            .WithErrorMessage("Category is required.");
    }
    
    [Fact]
    public void ShouldHaveError_WhenSubCategoryIdIsEmpty()
    {
        // Arrange
        var command = new CreateProductCommand(Name: "Product1", Price: 30, CategoryId: Guid.NewGuid(), SubCategoryId: Guid.Empty);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.SubCategoryId)
            .WithErrorMessage("Sub-category is required.");
    }


    [Fact]
    public void ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateProductCommand(Name: "Product1", Price: 30, CategoryId: Guid.NewGuid(), SubCategoryId: Guid.NewGuid());

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
        result.ShouldNotHaveValidationErrorFor(c => c.Price);
        result.ShouldNotHaveValidationErrorFor(c => c.CategoryId);
        result.ShouldNotHaveValidationErrorFor(c => c.SubCategoryId);
    }


}
