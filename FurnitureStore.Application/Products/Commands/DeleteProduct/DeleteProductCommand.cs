using ErrorOr;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid productId) : IRequest<ErrorOr<Deleted>>;