using FurnitureStore.Contracts.Products;
using FurnitureStore.Domain.Products;

using ContractProductColor = FurnitureStore.Contracts.Products.ProductColor;
using DomainProductColor = FurnitureStore.Domain.Products.ProductColor;

using ContractProductStatus = FurnitureStore.Contracts.Products.ProductStatus;
using DomainProductStatus = FurnitureStore.Domain.Products.ProductStatus;

namespace FurnitureStore.API.Utils;

public static class ProductHelper
{
    public static ProductResponse ToDto(Product product)
    {
        return new ProductResponse(
            Id: product.Id,
            Name: product.Name,
            Description: product.Description,
            Price: product.Price,
            CategoryName: product.Category?.Name ?? string.Empty,
            SubCategoryName: product.SubCategory?.Name ?? string.Empty,
            SKU: product.SKU,
            StockQuantity: product.StockQuantity,
            imageUrl: product.ImageUrl,
            Dimensions: product.Dimensions,
            Weight: product.Weight,
            Material: product.Material,
            Colors: product.Colors.ConvertAll(ToDto),
            Brand: product.Brand,
            Rating: product.Rating,
            IsFeatured: product.IsFeatured,
            Discount: product.Discount,
            Status: ToDto(product.Status));
    }

    private static ContractProductColor ToDto(DomainProductColor productColor)
    {
        return new ContractProductColor(
            ColorCode: productColor.Code,
            ColorName: productColor.Name);
    }

    private static ContractProductStatus ToDto(DomainProductStatus productStatus)
    {
        return productStatus.Name switch
        {
            nameof(DomainProductStatus.Active) => ContractProductStatus.Active,
            nameof(DomainProductStatus.Discontinued) => ContractProductStatus.Discontinued,
            nameof(DomainProductStatus.OutofStock) => ContractProductStatus.Discontinued,
            _ => throw new InvalidOperationException()
        };
    }
}
