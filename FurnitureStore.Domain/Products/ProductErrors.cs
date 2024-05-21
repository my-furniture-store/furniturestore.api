using ErrorOr;

namespace FurnitureStore.Domain.Products;

public class ProductErrors
{
    public static readonly Error CannotHaveDuplicateColors = Error.Validation(
       "Product.CannotHaveDuplicationColors",
       "A product cannot have duplicate colors.");
}
