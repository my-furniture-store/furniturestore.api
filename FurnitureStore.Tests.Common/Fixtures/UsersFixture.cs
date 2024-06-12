using FurnitureStore.Application.Utils;
using FurnitureStore.Domain.Users;

namespace FurnitureStore.Tests.Common.Fixtures;

public static class UsersFixture
{
    public static List<User> GetUsers()
    {
        var users = new List<User>();
    
        users.Add(CreateUser("john_doe", "johndoe@gmail.com", "John@Doe123", id: Guid.Parse("B69688AD-EAF8-44B9-8C86-1FC2485C5EB3")));
        users.Add(CreateUser("jane_doe", "janedoe@gmail.com", "JaneDoe123!", id: Guid.Parse("30B4735C-373B-48FB-9603-F860A5E1E889")));
        users.Add(CreateUser("alan_smith", "alan.smith@hotmail.com", "Alan1234!", id: Guid.Parse("30B4735C-373B-48FB-9603-F860A5E1E889")));
        users.Add(CreateUser("lucy_moore", "lucy.moore@hotmail.com", "Moore100?", id: Guid.Parse("1AB2A589-DE75-41DD-83E8-713F59EA7C9D")));

        return users;
    }


    private static User CreateUser(string username,string email, string password, Guid id)
    {
        PasswordManager.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        return new User(username, email, passwordHash, passwordSalt, id);
    }
    
}
