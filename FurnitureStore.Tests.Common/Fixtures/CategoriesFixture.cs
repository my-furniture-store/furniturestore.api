using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.SubCategories;
using FurnitureStore.Tests.Common.Helpers;

namespace FurnitureStore.Tests.Common.Fixtures;

public static class CategoriesFixture
{
    private static Category chairsCategory = new Category("Chairs", id: Guid.Parse("1D3A157B-9E49-461D-A78B-9B0207F2FA37"));

    public static List<Category> GetTestCategories()
    {
        EntityTestHelper.SetPrivateFieldValues<Category,List<SubCategory>>(chairsCategory, SubCategoriesFixture.GetTestSubCategories(chairsCategory.Id), "_subCategories");
        return new()
        {
            chairsCategory,
            new Category("Tables", id: Guid.Parse("7F55A4DE-D590-465C-9812-0D13DF122EC4")),
            new Category("Ornaments", id: Guid.Parse("CB6FD65A-600D-4A61-821C-2463E645665E")),
        };
    }
}
