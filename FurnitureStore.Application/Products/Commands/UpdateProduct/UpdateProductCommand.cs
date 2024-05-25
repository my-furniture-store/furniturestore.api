using ErrorOr;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(Guid ProductId, string Name, decimal Price): IRequest<ErrorOr<Product>>;

