using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Infrastructure.Categories;

public class CategoriesRepository : ICategoriesRepository
{
    private readonly FurnitureStoreDbContext _dbContext;

    public CategoriesRepository(FurnitureStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddCategoryAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
    }

    public async Task<bool> ExistsAsync(Guid categoryId)
    {
        return await _dbContext.Categories.AsNoTracking().AnyAsync(category => category.Id == categoryId);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid categoryId)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(category => category.Id == categoryId);
    }

    public Task RemoveCategoryAsync(Category category)
    {
        _dbContext.Remove(category);
        return Task.CompletedTask;
    }

    public Task UpdateCategoryAsync(Category category)
    {
        _dbContext.Update(category);
        return Task.CompletedTask;
    }
}
