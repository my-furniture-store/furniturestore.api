using ErrorOr;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Commands.DeleteSubCategory;

public record DeleteSubCategoryCommand(Guid CategoryId, Guid SubCategoryId): IRequest<ErrorOr<Deleted>>;

