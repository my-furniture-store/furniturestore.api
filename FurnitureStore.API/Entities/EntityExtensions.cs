using FurnitureStore.API.Dtos;

namespace FurnitureStore.API.Entities;

public static class EntityExtensions
{
    public static CategoryDto AsDto(this Category category)
        => new CategoryDto(category.Name);
}
