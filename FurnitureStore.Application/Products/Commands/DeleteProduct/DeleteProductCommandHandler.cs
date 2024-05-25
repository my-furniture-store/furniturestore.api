using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ErrorOr<Deleted>>
{
    private readonly IProductsRepository _productsRepository;
    private readonly IUnitofWork _unitofWork;

    public DeleteProductCommandHandler(IProductsRepository productsRepository, IUnitofWork unitofWork)
    {
        _productsRepository = productsRepository;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productsRepository.GetByIdAsync(request.productId);

        if ( product is null)
        {
            return Error.NotFound(description: "Product not found.");
        }

        await _productsRepository.RemoveProductAsync(product);
        await _unitofWork.CommitChangesAsync();

        return Result.Deleted;
    }
}
