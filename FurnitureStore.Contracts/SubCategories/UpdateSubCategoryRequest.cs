using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Contracts.SubCategories;

public record UpdateSubCategoryRequest([Required][StringLength(25)] string Name);

