using FurnitureStore.Domain.Products;
using FurnitureStore.Tests.Common.Helpers;

namespace FurnitureStore.Tests.Common.Fixtures;

public class ProductsFixture
{

    public static List<Product> GetListofProducts(Guid categoryId, Guid subCategoryId) => new()
    {
       new ProductBuilder()
            .WithId(Guid.Parse("9DE397BD-46DF-4E5B-A0F3-906B9ED1CD74"))
            .WithName("Product 1")
            .WithPrice(50m)
            .WithCategoryId(categoryId)
            .WithSubCategoryId(subCategoryId)
            .WithDescription("Description for Product 1")
            .WithSKU("SKU001")
            .WithStockQuantity(10)
            .WithImageUrl("http://example.com/product1.jpg")
            .WithDimensions("10x10x10")
            .WithWeight(5m)
            .WithMaterial("Wood")
            .WithColors(new List<ProductColor>
            {
                new ProductColor { Name = "Red", Code = "#FF0000" },
                new ProductColor { Name = "Blue", Code = "#0000FF" }
            })
            .WithBrand("Brand A")
            .WithRating(4.5)
            .WithDiscount(10m)
            .WithStatus(ProductStatus.Active)
            .Build(),

        new ProductBuilder()
            .WithId(Guid.Parse("D58481DC-CE1B-42C7-AF6D-6FFD1FD32817"))
            .WithName("Product 2")
            .WithPrice(75m)
            .WithCategoryId(categoryId)
            .WithSubCategoryId(subCategoryId)
            .WithDescription("Description for Product 2")
            .WithSKU("SKU002")
            .WithStockQuantity(0)
            .WithImageUrl("http://example.com/product2.jpg")
            .WithDimensions("15x15x15")
            .WithWeight(7m)
            .WithMaterial("Metal")
            .WithColors(new List<ProductColor>
            {
                new ProductColor { Name = "Green", Code = "#00FF00" },
                new ProductColor { Name = "Yellow", Code = "#FFFF00" }
            })
            .WithBrand("Brand B")
            .WithRating(4.0)
            .WithDiscount(5m)
            .WithStatus(ProductStatus.OutofStock)
            .Build(),

        new ProductBuilder()
            .WithId(Guid.Parse("0291B220-2635-40BD-8FE5-5F7CEBA6AD15"))
            .WithName("Product 3")
            .WithPrice(100m)
            .WithCategoryId(categoryId)
            .WithSubCategoryId(subCategoryId)
            .WithDescription("Description for Product 3")
            .WithSKU("SKU003")
            .WithStockQuantity(5)
            .WithImageUrl("http://example.com/product3.jpg")
            .WithDimensions("20x20x20")
            .WithWeight(10m)
            .WithMaterial("Plastic")
            .WithColors(new List<ProductColor>
            {
                new ProductColor { Name = "Black", Code = "#000000" },
                new ProductColor { Name = "White", Code = "#FFFFFF" }
            })
            .WithBrand("Brand C")
            .WithRating(3.5)
            .WithDiscount(15m)
            .WithStatus(ProductStatus.Discontinued)
            .Build()
    };


    public static Product CreateProduct(Product product)
    {
        return new ProductBuilder()
            .WithId(product.Id)
            .WithName(product.Name)
            .WithPrice(product.Price)
            .WithCategoryId(product.CategoryId)
            .WithSubCategoryId(product.SubCategoryId)
            .WithIsFeatured(product.IsFeatured)
            .WithDescription(product.Description)
            .WithSKU(product.SKU)
            .WithStockQuantity(product.StockQuantity)
            .WithImageUrl(product.ImageUrl)
            .WithDimensions(product.Dimensions)
            .WithWeight(product.Weight)
            .WithMaterial(product.Material)
            .WithColors(product.Colors)
            .WithBrand(product.Brand)
            .WithRating(product.Rating)
            .WithDiscount(product.Discount)
            .WithStatus(product.Status)
            .Build();
    }
}
