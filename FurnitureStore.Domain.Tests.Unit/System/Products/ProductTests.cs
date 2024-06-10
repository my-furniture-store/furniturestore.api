using FluentAssertions;
using FurnitureStore.Domain.Products;

namespace FurnitureStore.Domain.Tests.Unit.System.Products;

public class ProductTests
{
    private readonly Product _sut;
    private readonly Guid _categoryId = Guid.NewGuid();
    private readonly Guid _subCategoryId = Guid.NewGuid();

    public ProductTests()
    {
        _sut = new("Test Product", 10m, _categoryId, _subCategoryId);
    }

    [Fact]
    public void Constructor_ShouldInitializeProductCorrectly()
    {
        // Arrange
        var name = "Test Product";
        var price = 100m;
        var categoryId = Guid.NewGuid();
        var subCategoryId = Guid.NewGuid();

        // Act
        var product = new Product(name, price, categoryId, subCategoryId);

        // Assert
        product.Name.Should().Be(name);
        product.Price.Should().Be(price);
        product.CategoryId.Should().Be(categoryId);
        product.SubCategoryId.Should().Be(subCategoryId);
        product.DateAdded.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        product.IsFeatured.Should().BeFalse();
        product.Colors.Should().BeEmpty();
    }

    [Fact]
    public void UpdateProduct_ShouldUpdateNameAndPrice_WhenNameAndPriceAreProvided()
    {
        // Act
        _sut.UpdateProduct("New Name", 15m);

        // Assert
        _sut.Name.Should().Be("New Name");
        _sut.Price.Should().Be(15m);
    }

    [Fact]
    public void AddProductColor_ShouldAddColorToColors_WhenColorDoesNotExist()
    {
        // Act
        var result = _sut.AddProductColor("Red", "#FF0000");

        // Assert
        result.IsError.Should().BeFalse();
        _sut.Colors.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new ProductColor { Name = "Red", Code = "#FF0000" });
    }

    [Fact]
    public void AddProductColor_ShouldReturnError_WhenColorExists()
    {
        // Arrange
        _sut.AddProductColor("red", "#ff0000");

        // Act
        var result = _sut.AddProductColor("RED", "#FF0000");

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();
        result.Errors[0].Description.Should().Be("A product cannot have duplicate colors.");
    }

    [Fact]
    public void SetProductDescription_ShouldUpdateDescription()
    {
        // Act
        _sut.SetProductDescription("New Description");

        // Assert
        _sut.Description.Should().Be("New Description");
    }

    [Fact]
    public void SetStockQuantity_ShouldUpdateStockAndStatus()
    {
        // Act
        _sut.SetStockQuantity(10);

        // Assert
        _sut.StockQuantity.Should().Be(10);
        _sut.Status.Should().Be(ProductStatus.Active);
    }

    [Fact]
    public void SetDiscount_ShouldUpdateDiscount()
    {
        // Act
        _sut.SetDiscount(20m);

        // Assert
        _sut.Discount.Should().Be(20m);
    }

    [Fact]
    public void SetBrandAndMaterial_ShouldUpdateBrandAndMaterial()
    {
        // Act
        _sut.SetBrandAndMaterial("New Brand", "New Material");

        // Assert
        _sut.Brand.Should().Be("New Brand");
        _sut.Material.Should().Be("New Material");
    }

    [Fact]
    public void UpdateProductStatus_ShouldUpdateStatus()
    {
        // Act
        _sut.UpdateProductStatus(ProductStatus.Discontinued);

        // Assert
        _sut.Status.Should().Be(ProductStatus.Discontinued);
    }

    [Fact]
    public void SetImageUrl_ShouldUpdateImageUrl()
    {      
        // Act
        _sut.SetImageUrl("http://example.com/image.jpg");

        // Assert
        _sut.ImageUrl.Should().Be("http://example.com/image.jpg");
    }


    [Fact]
    public void SetRating_ShouldUpdateRating()
    {
        // Act
        _sut.SetRating(4.5);

        // Assert
        _sut.Rating.Should().Be(4.5);
    }

    [Fact]
    public void SetSKU_ShouldUpdateSKU()
    {
        // Arrange
        var skuValue = "PRO12345";

        // Act
        _sut.SetSKU(skuValue);

        // Assert
        _sut.SKU.Should().Be(skuValue);
    }

    [Fact]
    public void SetDimensionAndWeight_ShouldUpdateDimension_WhenDimensionIsProvided()
    {
        // Arrage
        var dimensions = "123cm * 45cm * 60cm";

        // Act
        _sut.SetDimensionAndWeight(dimensions);

        // Assert
        _sut.Dimensions.Should().Be(dimensions);
    }

    [Fact]
    public void SetDimensionAndWeight_ShouldUpdateWeight_WhenWeightIsProvided()
    {
        // Arrange
        var weight = 67;

        // Act
        _sut.SetDimensionAndWeight(weight: weight);

        // Assert
        _sut.Weight.Should().Be(weight);        
    }

}
