using ErrorOr;
using FurnitureStore.Domain.Categories;
using FurnitureStore.Domain.SubCategories;

namespace FurnitureStore.Domain.Products;

public class Product
{

    #region Constructors
    public Product(string name, decimal price, Guid categoryId, Guid subCategoryId, Guid? id = null, bool isFeatured = false)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Price = price;
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
        IsFeatured = isFeatured;
        DateAdded = DateTime.Now;
        Colors = new();
        Status = ProductStatus.Active;
    }

    private Product() { }
    #endregion Constructors

    #region Properties
    public Guid Id { get; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public Guid CategoryId { get; }
    public Guid SubCategoryId { get; }
    public string? SKU { get; private set; }
    public int? StockQuantity { get; private set; }
    public string? ImageUrl { get; private set; }
    public string? Dimensions { get; private set; }
    public decimal? Weight { get; private set; }
    public string? Material { get; private set; }
    public List<ProductColor> Colors { get; private set; } = new();
    public string? Brand { get; private set; }
    public double? Rating { get; private set; }
    public DateTime DateAdded { get; }
    public bool IsFeatured { get; private set; }
    public decimal? Discount { get; private set; }
    public ProductStatus Status { get; private set; } = null!;

    public Category Category { get; } = null!;
    public SubCategory SubCategory { get; } = null!;
    #endregion Properties

    #region Public Methods
    public void UpdateProduct(string? name = null, decimal? price = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            this.Name = name;
        }

        if (price != null)
        {
            this.Price = (decimal)price;
        }
    }

    public ErrorOr<Success> AddProductColor(string colorName, string colorCode)
    {
        var color = Colors?.Where(color => color.Name.ToLower() == colorName.ToLower() || color.Code.ToLower() == colorCode.ToLower()).FirstOrDefault();

        if (color is not null)
        {
            return ProductErrors.CannotHaveDuplicateColors;
        }

        Colors!.Add(new ProductColor { Name = colorName, Code = colorCode });

        return Result.Success;
    }

    public void SetProductDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return;

        this.Description = description;
    }

    public void UpdateProductStatus(ProductStatus? productStatus)
    {
        if (productStatus is null)
            return;

        this.Status = productStatus;
    }

    public void SetImageUrl(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return;

        this.ImageUrl = imageUrl;
    }

    public void SetBrandAndMaterial(string? brand = null, string? material = null)
    {
        if (!string.IsNullOrWhiteSpace(brand))
        {
            this.Brand = brand;
        }

        if (!string.IsNullOrWhiteSpace(material))
        {
            this.Material = material;
        }
    }

    public void SetStockQuantity(int? quantity)
    {
        this.StockQuantity = quantity;

        this.Status = ProductStatus.Active;
    }

    public void SetDiscount(decimal? discount)
    {
        this.Discount = discount;
    }
    #endregion Public Methods
}
