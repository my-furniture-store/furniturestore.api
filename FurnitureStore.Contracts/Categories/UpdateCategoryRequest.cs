using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Contracts.Categories;

public record UpdateCategoryRequest([Required][StringLength(50)]string Name);

