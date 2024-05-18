using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Domain.Tests.Unit.Fixtures;

public static class SubCategoriesFixture
{
    public static List<SubCategory> GetTestSubCategories(Guid categoryId) => new()
    {
        new SubCategory("Chesterfield", categoryId: categoryId, id:Guid.Parse("6E04E059-A132-43D7-8883-94C16F978945")),
        new SubCategory("Recliner", categoryId: categoryId, id: Guid.Parse("AB2C439E-0CB9-4F6F-A57D-BD592A951507")),
        new SubCategory("Sectional Sofa", categoryId: categoryId, id: Guid.Parse("5CA893FC-A76B-41E0-B366-6255E8821DBA"))

    };
}
