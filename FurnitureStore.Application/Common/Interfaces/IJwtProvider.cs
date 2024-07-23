using FurnitureStore.Domain.Users;

namespace FurnitureStore.Application.Common.Interfaces;

public interface IJwtProvider
{
    string GenerateUserAccessToken(User user, bool isResetPasswordToken = false);    
    bool ValidateToken(string token);
}
