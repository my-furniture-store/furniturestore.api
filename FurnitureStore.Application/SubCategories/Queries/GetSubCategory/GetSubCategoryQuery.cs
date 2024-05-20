using ErrorOr;
using FurnitureStore.Domain.SubCategories;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Queries.GetSubCategory;

public record GetSubCategoryQuery(Guid SubCategoryId, Guid CategoryId) : IRequest<ErrorOr<SubCategory>>;

