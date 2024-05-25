using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.SetProductDescription;

public class SetProductDescriptionCommandHandler : IRequestHandler<SetProductDescriptionCommand, ErrorOr<Product>>
{
    private readonly IProductsRepository _productsRepository;
    private readonly IUnitofWork _unitofWork;

    public SetProductDescriptionCommandHandler(IProductsRepository productsRepository, IUnitofWork unitofWork)
    {
        _productsRepository = productsRepository;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<Product>> Handle(SetProductDescriptionCommand request, CancellationToken cancellationToken)
    {
        var product = await _productsRepository.GetByIdAsync(request.productId);

        if (product is null)
        {
            return Error.NotFound(description: "Product not found.");
        }

        product.SetProductDescription(request.Description);

        await _productsRepository.UpdateAsync(product);
        await _unitofWork.CommitChangesAsync();

        return product;
    }
}
