using ErrorOr;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Queries.ListCategoryProducts;

public record ListCategoryProductsQuery(Guid CategoryId) : IRequest<ErrorOr<List<Product>>>;
