using FurnitureStore.Domain.Users;

namespace FurnitureStore.Application.Common.Interfaces;

public interface IUsersRepository
{
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task RemoveUserAsync(User user);
    Task<bool> UserExists(string email);
    Task<bool> IsUsernameUnique(string username);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByUsernameOrEmail(string? username = null, string? email = null);
}
