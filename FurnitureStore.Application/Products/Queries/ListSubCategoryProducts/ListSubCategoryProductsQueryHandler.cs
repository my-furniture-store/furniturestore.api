using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Queries.ListSubCategoryProducts;

public class ListSubCategoryProductsQueryHandler : IRequestHandler<ListSubCategoryProductsQuery, ErrorOr<List<Product>>>
{
    private readonly IProductsRepository _productsRepository;
    private readonly ISubCategoriesRepository _subCategoriesRepository;

    public ListSubCategoryProductsQueryHandler(IProductsRepository productsRepository, ISubCategoriesRepository subCategoriesRepository)
    {
        _productsRepository = productsRepository;
        _subCategoriesRepository = subCategoriesRepository;
    }

    public async Task<ErrorOr<List<Product>>> Handle(ListSubCategoryProductsQuery request, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoriesRepository.GetByIdAsync(request.SubCategoryId);

        if (subCategory is null)
            return Error.NotFound(description: "Sub-category not found.");

        var products = await _productsRepository.GetProductsBySubCategoryIdAsync(subCategory.Id);

        return products;
    }
}
