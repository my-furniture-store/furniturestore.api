using FurnitureStore.API.Entities;

namespace FurnitureStore.API.Repositories
{
    public interface ICategoryRepository
    {
        Task CreateAsync(Category category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetAsync(int id);
        Task<Category?> GetByIdAsync(int id);
        Task UpdateAsync(Category category);
    }
}