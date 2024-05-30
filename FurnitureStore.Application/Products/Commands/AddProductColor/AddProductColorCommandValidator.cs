using FluentValidation;

namespace FurnitureStore.Application.Products.Commands.AddProductColor;

public class AddProductColorCommandValidator :AbstractValidator<AddProductColorCommand>
{
    public AddProductColorCommandValidator()
    {
        RuleFor(command => command.ProductId)
            .NotEmpty().WithMessage("Product is required.");

        RuleFor(command => command.ColorName)
            .NotEmpty().WithMessage("Specify a color name.");

        RuleFor(command => command.ColorCode)
            .NotEmpty().WithMessage("Color code is required.")
            .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Color code must be in the format #FFFFFF or #ffffff.");
        
    }
}
