using FurnitureStore.Domain.Categories;

namespace FurnitureStore.Application.Common.Interfaces;

public interface ICategoriesRepository
{
    Task AddCategoryAsync(Category category);
    Task<bool> ExistsAsync(Guid categoryId);
    Task RemoveCategoryAsync(Category category);
    Task<Category?> GetByIdAsync(Guid categoryId);
    Task<List<Category>> GetAllCategoriesAsync();
    Task UpdateCategoryAsync(Category category);
}
