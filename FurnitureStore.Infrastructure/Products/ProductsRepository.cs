using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using FurnitureStore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Infrastructure.Products;

internal class ProductsRepository : IProductsRepository
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

    public async Task DeleteAsync(Guid productId)
    {
        var product = await GetByIdAsync(productId);

        if (product is null)
            return;

        _dbContext?.Products.Remove(product);        
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

    public async Task AddProductColorAsync(Guid productId, string colorName, string colorCode)
    {
        var product = await GetByIdAsync(productId);

        if (product is null)
            return;

        product.AddProductColor(colorName, colorCode);
    }
    

    public async Task SetBrandAndMaterialAsync(Guid productId, string? brand, string? material)
    {
        var product = await GetByIdAsync(productId);

        if (product is null)
            return;

        product.SetBrandAndMaterial(brand, material);
    }

    public async Task SetDiscountAsync(Guid productId, decimal discount)
    {
        var product = await GetByIdAsync(productId);

        if (product is null)
            return;

        product.SetDiscount(discount);
    }

    public async Task SetImageUrlAsync(Guid productId, string imageUrl)
    {
         var product = await GetByIdAsync(productId);

        if (product is null)
            return;

        product.SetImageUrl(imageUrl);
    }

    public async Task SetProductDescriptionAsync(Guid productId, string description)
    {
         var product = await GetByIdAsync(productId);

        if (product is null)
            return;

        product.SetProductDescription(description);
    }

    public async Task SetProductStatusAsync(Guid productId, ProductStatus status)
    {
         var product = await GetByIdAsync(productId);

        if (product is null)
            return;

        product.UpdateProductStatus(status);
    }

    public async Task SetStockQuantityAsync(Guid productId, int quantity)
    {
         var product = await GetByIdAsync(productId);

        if (product is null)
            return;

        product.SetStockQuantity(quantity);
    }

    
}
