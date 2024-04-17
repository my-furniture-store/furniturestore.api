using FurnitureStore.API.Dtos;
using FurnitureStore.API.Entities;
using FurnitureStore.API.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FurnitureStore.API.Endpoints;

public static class CategoryEndpoints
{
    private const string GetCategoriesEndpointName = "GetCategories";
    public static RouteGroupBuilder MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/categories")
                          .WithParameterValidation()
                          .WithTags("Categories");


        group.MapGet("/", async (ICategoryRepository repository) =>
        {
            var categories = await repository.GetAllAsync();
            return Results.Ok(categories.Select(c => c.AsDto()));
        })
        .WithSummary("Gets all categories")
        .WithDescription("Get all available categories");

        group.MapGet("/{id:int}", async Task<Results<Ok<CategoryDto>, NotFound>> (ICategoryRepository repository, int id) =>
        {
            Category? category = await repository.GetByIdAsync(id);

            if (category == null)
                return TypedResults.NotFound();

            return TypedResults.Ok(category.AsDto());
        })
        .WithName(GetCategoriesEndpointName)
        .WithSummary("Get Category by id")
        .WithDescription("Gets the category that has the specified id");

        group.MapPost("/", async Task<CreatedAtRoute<CategoryDto>> (ICategoryRepository repository, CategoryDto categoryDto) =>
        {
            Category category = new Category { Name = categoryDto.Name! };

            await repository.CreateAsync(category);

            return TypedResults.CreatedAtRoute(category.AsDto(), GetCategoriesEndpointName, new { category.Id });
        })
        .WithSummary("Creates a new category")
        .WithDescription("Creates a new category with specified properties");

        group.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (ICategoryRepository repository, int id, CategoryDto categoryDto) =>
        {
            Category? category = await repository.GetAsync(id);

            if (category is null)
                return TypedResults.NotFound();

            category.Name = categoryDto.Name!;

            await repository.UpdateAsync(category);

            return TypedResults.NoContent();
        })
        .WithSummary("Updates a Category")
        .WithDescription("Update properties of the category specified by the id");

        group.MapDelete("/{id}", async (ICategoryRepository repository, int id) =>
        {
            var categoryToDelete = await repository.GetAsync(id);

            if (categoryToDelete is not null)
                await repository.DeleteAsync(id);


            return TypedResults.NoContent();
        })
        .WithSummary("Deletes a category")
        .WithDescription("Deletes the category that has the specified id");

        return group;
    }
}
