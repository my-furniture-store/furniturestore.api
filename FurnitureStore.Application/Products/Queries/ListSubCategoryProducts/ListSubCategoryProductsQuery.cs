using ErrorOr;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Queries.ListSubCategoryProducts;

public record ListSubCategoryProductsQuery(Guid CategoryId, Guid SubCategoryId) : IRequest<ErrorOr<List<Product>>>;