using ErrorOr;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(string Name, decimal Price, Guid CategoryId, Guid SubCategoryId, bool isFeatured = false) : IRequest<ErrorOr<Product>>;


