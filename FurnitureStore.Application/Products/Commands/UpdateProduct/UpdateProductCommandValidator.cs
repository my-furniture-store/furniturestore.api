using FluentValidation;

namespace FurnitureStore.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
	public UpdateProductCommandValidator()
	{
        RuleFor(command => command.ProductId)
           .NotEmpty()
		   .WithMessage("Product is required.");

        When(command => command.Price.HasValue, () =>
		{
			RuleFor(command => command.Price)
            .GreaterThanOrEqualTo(25)
			.WithMessage("Product price must be at least 25.");
        });

		When(command => command.Colors?.Count > 0, () =>
		{
			RuleForEach(command => command.Colors).ChildRules(productColor =>
			{
				productColor.RuleFor(productColor => productColor.Code)
							.NotEmpty()
							.WithMessage("Color code is required.")
							.Matches("^#[0-9A-Fa-f]{6}$")
							.WithMessage("Color code must be in the format #FFFFFF or #ffffff.");

				productColor.RuleFor(productColor => productColor.Name)
							.NotEmpty()
							.WithMessage("Specify a color name.");
            });
		});
	}
}
