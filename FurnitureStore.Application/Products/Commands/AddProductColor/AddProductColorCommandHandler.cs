using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.AddProductColor;

public class AddProductColorCommandHandler : IRequestHandler<AddProductColorCommand, ErrorOr<Product>>
{
    private readonly IProductsRepository _productsRepository;
    private readonly IUnitofWork _unitofWork;

    public AddProductColorCommandHandler(IProductsRepository productsRepository, IUnitofWork unitofWork)
    {
        _productsRepository = productsRepository;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<Product>> Handle(AddProductColorCommand request, CancellationToken cancellationToken)
    {
        var product = await _productsRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            return Error.NotFound(description: "Product not found.");
        }

        var addProductColorResult = product.AddProductColor(request.ColorName, request.ColorCode);

        if (addProductColorResult.IsError)
            return addProductColorResult.Errors;

        await _productsRepository.UpdateAsync(product);
        await _unitofWork.CommitChangesAsync();

        return product;
    }
}
