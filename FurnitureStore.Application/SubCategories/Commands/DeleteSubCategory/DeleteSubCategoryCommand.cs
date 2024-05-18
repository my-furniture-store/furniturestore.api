using ErrorOr;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Commands.DeleteSubCategory;

public record DeleteSubCategoryCommand(Guid CategoryId, Guid Id): IRequest<ErrorOr<Deleted>>;

