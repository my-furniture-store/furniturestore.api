using FurnitureStore.Domain.Products;

namespace FurnitureStore.Tests.Common.Fixtures;

public class ProductsFixture
{
    private readonly Product _product1;
    private readonly Product _product2;
    private readonly Product _product3;


    public ProductsFixture()
    {
        _product1 = 1;
    }


    public static List<Product> GetListOfProduct(Guid categoryId, Guid subCategoryId) => new()
    {
        new Product("Product 1", 50m, categoryId, subCategoryId)
        {
            Description = "Description for Product 1",
            SKU = "SKU001",
            StockQuantity = 10,
            ImageUrl = "http://example.com/product1.jpg",
            Dimensions = "10x10x10",
            Weight = 5m,
            Material = "Wood",
            Colors = new List<ProductColor>
            {
                new ProductColor { Name = "Red", Code = "#FF0000" },
                new ProductColor { Name = "Blue", Code = "#0000FF" }
            },
            Brand = "Brand A",
            Rating = 4.5,
            Discount = 10m,
            Status = ProductStatus.Active
        },
        new Product("Product 2", 75m, categoryId, subCategoryId)
        {
            Description = "Description for Product 2",
            SKU = "SKU002",
            StockQuantity = 20,
            ImageUrl = "http://example.com/product2.jpg",
            Dimensions = "15x15x15",
            Weight = 7m,
            Material = "Metal",
            Colors = new List<ProductColor>
            {
                new ProductColor { Name = "Green", Code = "#00FF00" },
                new ProductColor { Name = "Yellow", Code = "#FFFF00" }
            },
            Brand = "Brand B",
            Rating = 4.0,
            Discount = 5m,
            Status = ProductStatus.Inactive
        },
        new Product("Product 3", 100m, categoryId, subCategoryId)
        {
            Description = "Description for Product 3",
            SKU = "SKU003",
            StockQuantity = 5,
            ImageUrl = "http://example.com/product3.jpg",
            Dimensions = "20x20x20",
            Weight = 10m,
            Material = "Plastic",
            Colors = new List<ProductColor>
            {
                new ProductColor { Name = "Black", Code = "#000000" },
                new ProductColor { Name = "White", Code = "#FFFFFF" }
            },
            Brand = "Brand C",
            Rating = 3.5,
            Discount = 15m,
            Status = ProductStatus.Discontinued
        }
    };
}
