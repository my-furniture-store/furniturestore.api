using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Products;
using MediatR;

namespace FurnitureStore.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ErrorOr<Product>>
{
    private readonly IProductsRepository _productsRepository;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly ISubCategoriesRepository _subCategoriesRepository;
    private readonly IUnitofWork _unitofWork;
    public CreateProductCommandHandler(IProductsRepository productsRepository, IUnitofWork unitofWork,
        ICategoriesRepository categoriesRepository, ISubCategoriesRepository subCategoriesRepository)
    {
        _productsRepository = productsRepository;
        _unitofWork = unitofWork;
        _categoriesRepository = categoriesRepository;
        _subCategoriesRepository = subCategoriesRepository;
    }

    public async Task<ErrorOr<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoriesRepository.GetByIdAsync(categoryId: request.CategoryId);

        if(category is null)
            return Error.NotFound(description: "Category not found.");

        var subCategory = await _subCategoriesRepository.GetByIdAsync(request.SubCategoryId);

        if(subCategory is null)
            return Error.NotFound(description: "Sub-category not found.");

        var product = new Product(request.Name, request.Price, request.CategoryId, request.SubCategoryId,isFeatured:request.isFeatured);

        await _productsRepository.AddAsync(product);
        await _unitofWork.CommitChangesAsync();

        return product;
    }
}
