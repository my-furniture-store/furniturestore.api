using ErrorOr;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Queries.ListFeaturedProducts;

public record ListFeaturedProductsQuery : IRequest<ErrorOr<List<Product>>>;
