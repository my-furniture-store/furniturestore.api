using System.Text.Json.Serialization;

namespace FurnitureStore.Contracts.Products;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProductStatus
{
    Active = 1,
    OutOfStock = 2,
    Discontinued = 3
}
