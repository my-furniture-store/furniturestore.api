using ErrorOr;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.AddProductColor;

public record AddProductColorCommand(Guid ProductId, string ColorName, string ColorCode):  IRequest<ErrorOr<Product>>;
