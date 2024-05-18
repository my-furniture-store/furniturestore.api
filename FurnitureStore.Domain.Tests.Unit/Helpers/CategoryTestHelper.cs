using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.SubCategories;
using System.Reflection;

namespace FurnitureStore.Domain.Tests.Unit.Helpers;

public static class CategoryTestHelper
{
    public static void SetSubCategories(Category category, List<SubCategory> subCategories)
    {
        var field = typeof(Category).GetField("_subCategories", BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(category, subCategories);
    }
}
