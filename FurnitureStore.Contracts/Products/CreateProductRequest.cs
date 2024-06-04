using FurnitureStore.Contracts.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Contracts.Products;

public record CreateProductRequest(
    [Required]
    [StringLength(150)]
    string Name,
    [Required]
    [MinimumValue(25)]
    decimal Price,
    [Required]
    Guid CategoryId,
    [Required]
    Guid SubCategoryId,
    bool IsFeatured = false
    );

