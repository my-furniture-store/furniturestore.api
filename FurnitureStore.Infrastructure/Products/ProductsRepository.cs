using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using FurnitureStore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Infrastructure.Products;

public class ProductsRepository : IProductsRepository
{
    private readonly FurnitureStoreDbContext _dbContext;

    public ProductsRepository(FurnitureStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetByIdAsync(Guid productId)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == productId);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
    }

    public Task UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);
        return Task.CompletedTask;
    }

    public Task RemoveProductAsync(Product product)
    {   
        _dbContext?.Products.Remove(product);
        return Task.CompletedTask;
    }

    public async Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId)
    {
        return await _dbContext.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsBySubCategoryIdAsync(Guid subCategoryId)
    {
        return await _dbContext.Products
            .Where(p => p.SubCategoryId == subCategoryId)
            .ToListAsync();
    }

    public async Task<List<Product>> GetFeaturedProductAsync()
    {
        return await _dbContext.Products
            .Where(p => p.IsFeatured)
            .ToListAsync();
    }
   
}
