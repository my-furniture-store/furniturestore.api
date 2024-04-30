using FurnitureStore.Domain.Categories;
using MediatR;

namespace FurnitureStore.Application.Categories.Queries.ListCategories;

public record ListCategoriesQuery : IRequest<IEnumerable<Category>>;

