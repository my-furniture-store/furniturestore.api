using ErrorOr;
using MediatR;

namespace FurnitureStore.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid CategoryId) : IRequest<ErrorOr<Deleted>>;
