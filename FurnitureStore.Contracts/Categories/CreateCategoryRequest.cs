using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Contracts.Categories;

public record CreateCategoryRequest([Required][StringLength(50)]string Name);
