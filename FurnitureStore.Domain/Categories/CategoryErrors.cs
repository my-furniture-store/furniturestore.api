using ErrorOr;

namespace FurnitureStore.Domain.Categories;

public class CategoryErrors
{
    public static readonly Error CannotHaveDuplicateSubCategories = Error.Validation(
        "Category.CannotHaveDuplicationSubCategories",
        "A category cannot have duplicate sub-categories").
}
