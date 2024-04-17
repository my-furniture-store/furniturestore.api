using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.API.Entities;

public class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public required string Name { get; set; }
}
