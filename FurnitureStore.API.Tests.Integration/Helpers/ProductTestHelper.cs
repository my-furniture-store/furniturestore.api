using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using FurnitureStore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FurnitureStore.API.Tests.Integration.Helpers;

public static  class ProductTestHelper
{
    public static async Task CreateProduct(FurnistoreApiFactory appFactory, params Product[] products)
    {
        using (var scope = appFactory.Services.CreateScope())
        {
            var productsRepo = scope.ServiceProvider.GetRequiredService<IProductsRepository>();
            var unitofWork = scope.ServiceProvider.GetRequiredService<IUnitofWork>();

            foreach (var product in products)
            {
                await productsRepo.AddAsync(product);
                await unitofWork.CommitChangesAsync();
            }

        }
    }

    public static async Task ClearAllProducts(FurnistoreApiFactory appFactory)
    {
        using (var scope = appFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<FurnitureStoreDbContext>();       

            var categories = await dbContext.Products.ToListAsync();
            dbContext.Products.RemoveRange(categories);

            await dbContext.SaveChangesAsync();

        }
    }
}
