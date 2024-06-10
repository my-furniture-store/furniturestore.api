using FluentValidation.TestHelper;
using FurnitureStore.Application.Products.Commands.UpdateProduct;
using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Validators;

public class UpdateProductCommandValidatorTests
{
    private readonly UpdateProductCommandValidator _sut;

    public UpdateProductCommandValidatorTests()
    {
        _sut = new UpdateProductCommandValidator();
    }

    [Fact]
    public void ShouldHaveError_WhenProductIdIsEmpty()
    {
        // Arrange
        var command = new UpdateProductCommand(ProductId: Guid.Empty);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.ProductId)
            .WithErrorMessage("Product is required.");
    }


    [Fact]
    public void ShouldNotHaveError_WhenProductIdIsProvided()
    {
        // Arrange
        var command = new UpdateProductCommand(ProductId: Guid.NewGuid());

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.ProductId);
    }


    [Fact]
    public void ShouldHaveError_WhenPriceIsLessThan25()
    {
        // Arrange
        var command = new UpdateProductCommand(ProductId: Guid.NewGuid(),Price: 24);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Price)
           .WithErrorMessage("Product price must be at least 25.");
    }
    
    [Fact]
    public void ShouldNotHaveError_WhenPriceIs25OrMore()
    {
        // Arrange
        var command = new UpdateProductCommand(ProductId: Guid.NewGuid(),Price: 25);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Price);           
    }


    [Fact]
    public void ShouldHaveError_WhenColorsAreProvidedWithoutNameAndCode()
    {
        // Arrange
        var colors = new List<ProductColor>
        {
            new ProductColor {Name = "", Code = ""},
            new ProductColor {Name = "", Code = ""}
        };

        var command = new UpdateProductCommand(ProductId: Guid.NewGuid(), Colors: colors);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Colors[0].Code").WithErrorMessage("Color code is required.");
        result.ShouldHaveValidationErrorFor("Colors[0].Name").WithErrorMessage("Specify a color name.");

    }
    
    [Fact]
    public void ShouldHaveError_WhenColorCodeIsInvalid()
    {
        // Arrange
        var colors = new List<ProductColor>
        {
            new ProductColor {Name = "Red", Code = "ff4434"}
        };

        var command = new UpdateProductCommand(ProductId: Guid.NewGuid(), Colors: colors);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Colors[0].Code")
            .WithErrorMessage("Color code must be in the format #FFFFFF or #ffffff.");

    }
    
    [Fact]
    public void ShouldNotHaveError_WhenColorCodeAndNameAreValid()
    {
        // Arrange
        var colors = new List<ProductColor>
        {
            new ProductColor {Name = "Red", Code = "#ff4434"}
        };

        var command = new UpdateProductCommand(ProductId: Guid.NewGuid(), Colors: colors);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor("Colors[0].Code");
        result.ShouldNotHaveValidationErrorFor("Colors[0].Name");
    }

    [Fact]
    public void ShouldNotHaveError_WhenColorsAreNotProvided()
    {
        // Arrange
        var command = new UpdateProductCommand(ProductId: Guid.NewGuid(), Colors: null);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(command => command.Colors);
    }





}
