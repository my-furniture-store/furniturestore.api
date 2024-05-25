using FurnitureStore.Domain.Products;

namespace FurnitureStore.Application.Common.Interfaces;

public interface IProductsRepository
{
    Task<Product?> GetByIdAsync(Guid productId);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetFeaturedProductAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task RemoveProductAsync(Product product);

    Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId);
    Task<List<Product>> GetProductsBySubCategoryIdAsync(Guid subCategoryId);    
}
