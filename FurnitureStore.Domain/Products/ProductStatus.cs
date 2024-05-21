using Ardalis.SmartEnum;

namespace FurnitureStore.Domain.Products;

public class ProductStatus : SmartEnum<ProductStatus>
{
    public static readonly ProductStatus Active = new(nameof(Active), 1);
    public static readonly ProductStatus OutofStock = new(nameof(OutofStock), 2);
    public static readonly ProductStatus Discontinued = new(nameof(Discontinued), 3);

    public ProductStatus(string name, int value) : base(name, value)
    {
    }
}
