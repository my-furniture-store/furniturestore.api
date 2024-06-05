using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Queries.ListSubCategoryProducts;

public class ListSubCategoryProductsQueryHandler : IRequestHandler<ListSubCategoryProductsQuery, ErrorOr<List<Product>>>
{
    private readonly IProductsRepository _productsRepository;
    private readonly ICategoriesRepository _categoriesRepository;

    public ListSubCategoryProductsQueryHandler(IProductsRepository productsRepository, ICategoriesRepository categoriesRepository)
    {
        _productsRepository = productsRepository;
        _categoriesRepository = categoriesRepository;
    }

    public async Task<ErrorOr<List<Product>>> Handle(ListSubCategoryProductsQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoriesRepository.GetByIdAsync(request.CategoryId);

        if (category == null)
            return Error.NotFound(description:"Category not found.");


        if (!category.HasSubCategory(request.SubCategoryId))
            return Error.NotFound(description: "Sub-category not found.");

        var products = await _productsRepository.GetProductsBySubCategoryIdAsync(request.SubCategoryId);

        return products;
    }
}
