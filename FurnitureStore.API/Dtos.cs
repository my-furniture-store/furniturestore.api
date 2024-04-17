using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.API.Dtos;

public record CategoryDto
{
    public int Id { get; init; }
    [Required]
    [StringLength(20)]
    public string? Name { get; init; }
}
