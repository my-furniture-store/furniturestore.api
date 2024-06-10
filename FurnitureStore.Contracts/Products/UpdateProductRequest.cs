using FurnitureStore.Contracts.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Contracts.Products;

public record UpdateProductRequest(
    string? Name = null,
    [MinimumValue(25)]decimal? Price = null,
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
    ProductStatus? ProductStatus = null);

