using ErrorOr;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Queries.ListProducts;

public record ListProductsQuery :IRequest<ErrorOr<List<Product>>>;