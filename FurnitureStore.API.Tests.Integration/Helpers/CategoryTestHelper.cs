using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FurnitureStore.API.Tests.Integration.Helpers;

public static class CategoryTestHelper
{
    public static async Task CreateCategory(FurnistoreApiFactory appFactory,params Category[] categories)
    {
        using (var scope = appFactory.Services.CreateScope())
        {
            var categoryRepo = scope.ServiceProvider.GetRequiredService<ICategoriesRepository>();

            foreach(var category in categories) 
            {
                await categoryRepo.AddCategoryAsync(category);
            }

        }
    }

    public static async Task ClearAllCategories(FurnistoreApiFactory appFactory)
    {
        using (var scope = appFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<FurnitureStoreDbContext>();

            var categories = await dbContext.Categories.ToListAsync();
            dbContext.Categories.RemoveRange(categories);       
            
            await dbContext.SaveChangesAsync();

        }
    }
}
