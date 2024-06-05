using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.SubCategories;
using FurnitureStore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FurnitureStore.API.Tests.Integration.Helpers;

public static class SubCategoryTestHelper
{
    public static async Task CreateSubCategory(FurnistoreApiFactory appFactory, params SubCategory[] subCategories)
    {
        using (var scope = appFactory.Services.CreateScope())
        {
            var subCategoryRepo = scope.ServiceProvider.GetRequiredService<ISubCategoriesRepository>();
            var unitofWork = scope.ServiceProvider.GetRequiredService<IUnitofWork>();

            foreach (var subCategory in subCategories)
            {
                await subCategoryRepo.AddSubCategoryAsync(subCategory);
                await unitofWork.CommitChangesAsync();
            }

        }
    }

    public static async Task ClearAllSubCategories(FurnistoreApiFactory appFactory)
    {
        using (var scope = appFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<FurnitureStoreDbContext>();

            var subCategories = await dbContext.SubCategories.ToListAsync();
            dbContext.SubCategories.RemoveRange(subCategories);

            await dbContext.SaveChangesAsync();

        }
    }
}
