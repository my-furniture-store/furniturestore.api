using FurnitureStore.API.Data;
using FurnitureStore.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FurnitureStore.API.Tests.Integration.Helpers;

public static class CategoryTestHelper
{
    public static async Task CreateCategory(FurnistoreApiFactory appFactory,params Category[] categories)
    {
        using (var scope = appFactory.Services.CreateScope())
        {
            var categoryRepo = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();

            foreach(var category in categories) 
            {
                await categoryRepo.CreateAsync(category);
            }

        }
    }

    public static async Task ClearAllCategories(FurnistoreApiFactory appFactory)
    {
        using (var scope = appFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<FurnitureStoreContext>();

            var categories = await dbContext.Categories.ToListAsync();
            dbContext.Categories.RemoveRange(categories);       
            
            await dbContext.SaveChangesAsync();

        }
    }
}
