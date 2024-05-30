using ErrorOr;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Queries.GetProduct;

public record GetProductQuery(Guid ProductId) : IRequest<ErrorOr<Product>>;