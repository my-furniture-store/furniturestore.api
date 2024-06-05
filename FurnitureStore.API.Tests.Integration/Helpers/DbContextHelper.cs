namespace FurnitureStore.API.Tests.Integration.Helpers;

public static class DbContextHelper
{
    public static async Task ClearAllTables(FurnistoreApiFactory appFactory)
    {
        await ProductTestHelper.ClearAllProducts(appFactory);
        await SubCategoryTestHelper.ClearAllSubCategories(appFactory);
        await CategoryTestHelper.ClearAllCategories(appFactory);
    }
}
