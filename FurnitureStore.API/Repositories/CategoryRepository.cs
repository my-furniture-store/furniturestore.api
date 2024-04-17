using FurnitureStore.API.Data;
using FurnitureStore.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.API.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly FurnitureStoreContext _storeContext;
    public CategoryRepository(FurnitureStoreContext storeContext)
    {
        _storeContext = storeContext;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _storeContext.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _storeContext.Categories.FindAsync(id);
    }

    public async Task<Category?> GetAsync(int id)
    {
        return await _storeContext.Categories.FindAsync(id);
    }

    public async Task CreateAsync(Category category)
    {
        _storeContext.Categories.Add(category);
        await _storeContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _storeContext.Update(category);
        await _storeContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _storeContext.Categories.Where(c => c.Id == id).ExecuteDeleteAsync();
    }
}
