using ErrorOr;
using FurnitureStore.Domain.Categories;
using MediatR;

namespace FurnitureStore.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name) : IRequest<ErrorOr<Category>>;
