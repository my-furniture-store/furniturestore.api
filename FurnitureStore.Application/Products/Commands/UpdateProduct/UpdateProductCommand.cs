using ErrorOr;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid ProductId,
    string? Name = null,
    decimal? Price = null,
    bool? IsFeatured = null,
    string? Description = null,
    string? SKU = null,
    int? StockQuantity = null,
    string? ImageUrl = null,
    string? Dimensions = null,
    decimal? Weight = null,
    string? Material = null,
    List<ProductColor>? Colors = null,
    string? Brand = null,
    double? Rating = null,
    decimal? Discount = null,
    ProductStatus? ProductStatus = null
    ): IRequest<ErrorOr<Product>>;

