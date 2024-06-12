using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Users;
using FurnitureStore.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace FurnitureStore.Infrastructure.Users;

public class UsersRepository : IUsersRepository
{
    private readonly FurnitureStoreDbContext _dbContext;

    public UsersRepository(FurnitureStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateUserAsync(User user)
    {
       await _dbContext.Users.AddAsync(user);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
    }

    public Task RemoveUserAsync(User user)
    {
        _dbContext.Users.Remove(user);
        return Task.CompletedTask;
    }

    public Task UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task<bool> UserExists(string email)
    {
        return await _dbContext.Users.AsNoTracking().AnyAsync(user => user.Email == email);
    }

    public async Task<bool> UsernameUsed(string username)
    {
        return await _dbContext.Users.AsNoTracking().AnyAsync(user => user.Username == username);
    }
}
