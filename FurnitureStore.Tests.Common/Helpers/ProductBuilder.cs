using FurnitureStore.Domain.Products;

namespace FurnitureStore.Tests.Common.Helpers;

public class ProductBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _name = "Default Name";
    private string _description = "Default Description";
    private decimal _price = 25m;
    private Guid _categoryId = Guid.NewGuid();
    private Guid _subCategoryId = Guid.NewGuid();
    private string _sku = "Default SKU";
    private int _stockQuantity = 0;
    private string _imageUrl = "http://example.com/default.jpg";
    private string _dimensions = "0x0x0";
    private decimal _weight = 0m;
    private string _material = "Default Material";
    private List<ProductColor> _colors = new List<ProductColor>();
    private string _brand = "Default Brand";
    private double _rating = 0;
    private bool _isFeatured = false;
    private decimal _discount = 0m;
    private ProductStatus _status = ProductStatus.Active;


    public ProductBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductBuilder WithCategoryId(Guid categoryId)
    {
        _categoryId = categoryId;
        return this;
    }

    public ProductBuilder WithSubCategoryId(Guid subCategoryId)
    {
        _subCategoryId = subCategoryId;
        return this;
    }

    public ProductBuilder WithSKU(string sku)
    {
        _sku = sku;
        return this;
    }

    public ProductBuilder WithStockQuantity(int stockQuantity)
    {
        _stockQuantity = stockQuantity;
        return this;
    }

    public ProductBuilder WithImageUrl(string imageUrl)
    {
        _imageUrl = imageUrl;
        return this;
    }

    public ProductBuilder WithDimensions(string dimensions)
    {
        _dimensions = dimensions;
        return this;
    }

    public ProductBuilder WithWeight(decimal weight)
    {
        _weight = weight;
        return this;
    }

    public ProductBuilder WithMaterial(string material)
    {
        _material = material;
        return this;
    }

    public ProductBuilder WithColors(List<ProductColor> colors)
    {
        _colors = colors;
        return this;
    }

    public ProductBuilder WithBrand(string brand)
    {
        _brand = brand;
        return this;
    }

    public ProductBuilder WithRating(double rating)
    {
        _rating = rating;
        return this;
    }

    public ProductBuilder WithIsFeatured(bool isFeatured)
    {
        _isFeatured = isFeatured;
        return this;
    }

    public ProductBuilder WithDiscount(decimal discount)
    {
        _discount = discount;
        return this;
    }

    public ProductBuilder WithStatus(ProductStatus status)
    {
        _status = status;
        return this;
    }

    public Product Build()
    {
        var product = new Product(_name, _price, _categoryId, _subCategoryId,_id);
        product.GetType()!.GetProperty("Description")!.SetValue(product, _description);
        product.GetType()!.GetProperty("SKU")!.SetValue(product, _sku);
        product.GetType()!.GetProperty("StockQuantity")!.SetValue(product, _stockQuantity);
        product.GetType()!.GetProperty("ImageUrl")!.SetValue(product, _imageUrl);
        product.GetType()!.GetProperty("Dimensions")!.SetValue(product, _dimensions);
        product.GetType()!.GetProperty("Weight")!.SetValue(product, _weight);
        product.GetType()!.GetProperty("Material")!.SetValue(product, _material);
        product.GetType()!.GetProperty("Colors")!.SetValue(product, _colors);
        product.GetType()!.GetProperty("Brand")!.SetValue(product, _brand);
        product.GetType()!.GetProperty("Rating")!.SetValue(product, _rating);
        product.GetType()!.GetProperty("IsFeatured")!.SetValue(product, _isFeatured);
        product.GetType()!.GetProperty("Discount")!.SetValue(product, _discount);
        product.GetType()!.GetProperty("Status")!.SetValue(product, _status);

        return product;
    }
}
