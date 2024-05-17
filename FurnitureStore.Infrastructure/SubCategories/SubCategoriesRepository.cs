using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Infrastructure.SubCategories;

public class SubCategoriesRepository : ISubCategoriesRepository
{
    private readonly FurnitureStoreDbContext _dbContext;

    public SubCategoriesRepository(FurnitureStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddSubCategoryAsync(SubCategory subCategory)
    {
        await _dbContext.SubCategories.AddAsync(subCategory);
    }

    public Task RemoveSubCategoryAsync(SubCategory subCategory)
    {
        _dbContext.SubCategories.Remove(subCategory);
        return Task.CompletedTask;
    }

    public async Task<SubCategory?> GetByIdAsync(Guid id)
    {
        return await _dbContext.SubCategories.FirstOrDefaultAsync(subCategory => subCategory.Id == id);
    }

    public async Task<List<SubCategory>> ListByCategoryIdAsync(Guid categoryId)
    {
        return await _dbContext.SubCategories
                    .Where(subCategory => subCategory.CategoryId == categoryId)
                    .ToListAsync();
    }

    public Task UpdateAsync(SubCategory subCategory)
    {
        _dbContext.Update(subCategory);
        return Task.CompletedTask;
    }
}
