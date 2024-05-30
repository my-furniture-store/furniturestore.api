using FluentValidation;

namespace FurnitureStore.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty().WithMessage("Product name is required.");

        RuleFor(product => product.Price)
            .GreaterThanOrEqualTo(25).WithMessage("Product price must be at least 25.");

        RuleFor(product => product.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(product => product.SubCategoryId)
            .NotEmpty().WithMessage("Sub-category is required.");
    }
}
