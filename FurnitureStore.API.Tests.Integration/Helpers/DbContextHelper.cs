using FurnitureStore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FurnitureStore.API.Tests.Integration.Helpers;

public static class DbContextHelper
{
    public static async Task ClearAllTables(FurnistoreApiFactory appFactory)
    {
        await ProductTestHelper.ClearAllProducts(appFactory);
        await SubCategoryTestHelper.ClearAllSubCategories(appFactory);
        await CategoryTestHelper.ClearAllCategories(appFactory);
    }

    public static async Task CreateEntity<TEntity>(FurnistoreApiFactory appFactory, params TEntity[] entities) where TEntity : class
    {
        using (var scope = appFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<FurnitureStoreDbContext>();

            foreach (var entity in entities)
            {
                dbContext.Set<TEntity>().Add(entity);
            }
            await dbContext.SaveChangesAsync();
        }
    }

    public static async Task ClearEntities<TEntity>(FurnistoreApiFactory appFactory) where TEntity : class
    {
        using (var scope = appFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<FurnitureStoreDbContext>();
            var dbSet = dbContext.Set<TEntity>();

            var entities = await dbSet.ToListAsync();
            dbSet.RemoveRange(entities);
            
            await dbContext.SaveChangesAsync();
        }
    }
}
