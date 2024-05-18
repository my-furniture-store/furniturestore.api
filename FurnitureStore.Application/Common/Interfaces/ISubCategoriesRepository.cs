using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Application.Common.Interfaces;

public interface ISubCategoriesRepository
{
    Task AddSubCategoryAsync(SubCategory subCategory);
    Task<SubCategory?> GetByIdAsync(Guid id);
    Task<List<SubCategory>> ListByCategoryIdAsync(Guid categoryId);
    Task UpdateAsync(SubCategory subCategory);
    Task RemoveSubCategoryAsync(SubCategory subCategory);
}
