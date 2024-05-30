using FluentValidation.TestHelper;
using FurnitureStore.Application.Products.Commands.AddProductColor;

namespace FurnitureStore.Application.Tests.Unit.System.Products.Validators;

public class AddProductColorCommandValidatorTests
{
    public readonly AddProductColorCommandValidator _sut;

    public AddProductColorCommandValidatorTests()
    {
        _sut = new AddProductColorCommandValidator();
    }


    [Fact]
    public void ShouldHaveError_WhenProductIdIsEmpty()
    {
        // Arrange
        var command = new AddProductColorCommand(Guid.Empty, "White", "#FFFFFF");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ProductId)
            .WithErrorMessage("Product is required.");
    }


    [Fact]
    public void ShouldHaveError_WhenColorNameIsEmpty()
    {
        // Arrange
        var command = new AddProductColorCommand(Guid.NewGuid(), "", "#ff0000");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ColorName)
            .WithErrorMessage("Specify a color name.");
    }

    [Fact]
    public void ShouldHaveError_WhenColorCodeIsEmpty()
    {
        // Arrange
        var command = new AddProductColorCommand(Guid.NewGuid(), "Red", "");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ColorCode)
            .WithErrorMessage("Color code is required.");
    }


    [Fact]
    public void ShouldHaveError_WhenColorCodeIsInvalid()
    {
        // Arrange
        var command = new AddProductColorCommand(Guid.NewGuid(), "Red", "445521");

        // Act
        var result = _sut.TestValidate(command);


        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ColorCode)
            .WithErrorMessage("Color code must be in the format #FFFFFF or #ffffff.");
    }

    [Fact]
    public void ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new AddProductColorCommand(Guid.NewGuid(), "Red", "#ff0000");

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.ProductId);
        result.ShouldNotHaveValidationErrorFor(c => c.ColorName);
        result.ShouldNotHaveValidationErrorFor(c => c.ColorCode);
    }

}


