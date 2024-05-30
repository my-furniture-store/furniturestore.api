using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Queries.ListProducts;

public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, ErrorOr<List<Product>>>
{
    private readonly IProductsRepository _productsRepository;

    public ListProductsQueryHandler(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }

    public async Task<ErrorOr<List<Product>>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productsRepository.GetAllAsync();

        return products;
    }
}
