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

        await _productsRepository.UpdateAsync(product);
        await _unitofWork.CommitChangesAsync();

        return product;
    }
}
