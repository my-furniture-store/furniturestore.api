using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Contracts.Authentication;

public record RegisterRequest(
    [Required]
    string Username,
    [Required]
    [EmailAddress]
    string Email,
    [Required]
    string Password
   );

