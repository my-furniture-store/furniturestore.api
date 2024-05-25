using ErrorOr;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.SetProductDescription;

public record SetProductDescriptionCommand(Guid productId, string Description) : IRequest<ErrorOr<Product>>;
