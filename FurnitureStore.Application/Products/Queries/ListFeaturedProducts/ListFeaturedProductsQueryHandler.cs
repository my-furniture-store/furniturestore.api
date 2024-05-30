using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Queries.ListFeaturedProducts;

public class ListFeaturedProductsQueryHandler : IRequestHandler<ListFeaturedProductsQuery, ErrorOr<List<Product>>>
{
    private readonly IProductsRepository _productsRepository;

    public ListFeaturedProductsQueryHandler(IProductsRepository productsRepository)
    {
        _productsRepository = productsRepository;
    }

    public async Task<ErrorOr<List<Product>>> Handle(ListFeaturedProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productsRepository.GetFeaturedProductAsync();

        return products;
    }
}
