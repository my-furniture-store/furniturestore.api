using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Contracts.Authentication;

public record LoginRequest([Required]string Password, string? Username = null, string? Email = null );