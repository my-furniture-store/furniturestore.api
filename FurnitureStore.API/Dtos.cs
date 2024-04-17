using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.API.Dtos;

public record CategoryDto(
    [Required][StringLength(20)]string Name);
