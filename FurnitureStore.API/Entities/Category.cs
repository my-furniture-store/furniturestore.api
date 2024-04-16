using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.API.Entities;

public class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Name { get; set; }
}
