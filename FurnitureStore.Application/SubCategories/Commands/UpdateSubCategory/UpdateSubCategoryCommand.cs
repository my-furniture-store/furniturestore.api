using ErrorOr;
using FurnitureStore.Domain.SubCategories;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Commands.UpdateSubCategory;

public record UpdateSubCategoryCommand(Guid CategoryId, Guid SubCategoryId,  string NewName) : IRequest<ErrorOr<SubCategory>>;

