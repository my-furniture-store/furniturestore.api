using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Common.Interfaces;

public interface IProductsRepository
{
    Task<Product?> GetByIdAsync(Guid productId);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetFeaturedProductAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid productId);

    Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId);
    Task<List<Product>> GetProductsBySubCategoryIdAsync(Guid subCategoryId);

    Task AddProductColorAsync(Guid productId, string colorName, string colorCode);
    Task SetProductDescriptionAsync(Guid productId, string description);
    Task SetImageUrlAsync(Guid productId, string imageUrl);
    Task SetBrandAndMaterialAsync(Guid productId, string? brand, string? material);
    Task SetStockQuantityAsync(Guid productId, int quantity);
    Task SetDiscountAsync(Guid productId, decimal discount);
    Task SetProductStatusAsync(Guid productId, ProductStatus status);
}
