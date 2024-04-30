using ErrorOr;
using FurnitureStore.Domain.Categories;
using MediatR;

namespace FurnitureStore.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid CategoryId, string CategoryName) : IRequest<ErrorOr<Category>>;

