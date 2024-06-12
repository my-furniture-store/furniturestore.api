using ErrorOr;

namespace FurnitureStore.Domain.Users;

public class UserErrors
{
    public static readonly Error InvalidPasswordHash =
        Error.Validation(
            "User.InvalidPasswordHash",
            "Invalid password hash.");
    
    public static readonly Error InvalidPasswordSalt =
        Error.Validation(
            "User.InvalidPasswordSalt",
            "Invalid password salt.");
}
