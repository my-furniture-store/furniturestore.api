using ErrorOr;
using FurnitureStore.Domain.SubCategories;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Queries.ListSubCategories;

public record ListSubCategoriesQuery(Guid CategoryId) : IRequest<ErrorOr<List<SubCategory>>>;
