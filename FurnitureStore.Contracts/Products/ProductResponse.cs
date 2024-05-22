namespace FurnitureStore.Contracts.Products;

public record ProductResponse(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    string CategoryName,
    string SubCategoryName,
    string? SKU,
    int? StockQuantity,
    string? imageUrl,
    string? Dimensions,
    decimal? Weight,
    string? Material,
    List<ProductColor> Colors,
    string? Brand,
    double? Rating,
    bool IsFeatured,
    decimal? Discount,
    ProductStatus Status
    );

