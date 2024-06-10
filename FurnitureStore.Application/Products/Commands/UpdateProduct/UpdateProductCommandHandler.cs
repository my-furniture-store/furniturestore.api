using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ErrorOr<Product>>
{
    private readonly IProductsRepository _productsRepository;
    private readonly IUnitofWork _unitofWork;

    public UpdateProductCommandHandler(IProductsRepository productsRepository, IUnitofWork unitofWork)
    {
        _productsRepository = productsRepository;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productsRepository.GetByIdAsync(request.ProductId);

        if(product is null)
        {
            return Error.NotFound(description: "Product not found.");
        }

        product.UpdateProduct(request.Name, request.Price);
        product.SetProductDescription(request.Description);
        product.UpdateProductStatus(request.ProductStatus);
        product.SetImageUrl(request.ImageUrl);
        product.SetBrandAndMaterial(request.Brand, request.Material);
        product.SetStockQuantity(request.StockQuantity);
        product.SetDiscount(request.Discount);
        product.SetRating(request.Rating);
        product.SetSKU(request.SKU);
        product.SetDimensionAndWeight(dimensions: request.Dimensions, weight: request.Weight);

        // add colors
        if(request.Colors != null && request.Colors.Count > 0)
        {
            foreach(var color in request.Colors)
            {
               var result = product.AddProductColor(colorName: color.Name, colorCode: color.Code);

                if (result.IsError) return result.Errors;
            }
        }

        await _productsRepository.UpdateAsync(product);
        await _unitofWork.CommitChangesAsync();

        return product;
    }
}
