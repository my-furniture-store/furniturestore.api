using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Contracts.SubCategories;

public record CreateSubCategoryRequest([Required][StringLength(25)]string Name);

