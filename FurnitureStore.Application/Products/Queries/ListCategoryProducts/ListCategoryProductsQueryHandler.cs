using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Queries.ListCategoryProducts;

public class ListCategoryProductsQueryHandler : IRequestHandler<ListCategoryProductsQuery, ErrorOr<List<Product>>>
{
    private readonly IProductsRepository _productsRepository;
    private readonly ICategoriesRepository _categoriesRepository;

    public ListCategoryProductsQueryHandler(IProductsRepository productsRepository, ICategoriesRepository categoriesRepository)
    {
        _productsRepository = productsRepository;
        _categoriesRepository = categoriesRepository;
    }

    public async Task<ErrorOr<List<Product>>> Handle(ListCategoryProductsQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoriesRepository.GetByIdAsync(request.CategoryId);

        if (category is null)
            return Error.NotFound(description: "Category not found.");

        var products = await _productsRepository.GetProductsByCategoryIdAsync(category.Id);

        return products;
    }
}
