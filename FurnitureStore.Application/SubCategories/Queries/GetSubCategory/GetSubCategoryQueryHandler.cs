using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.SubCategories;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Queries.GetSubCategory;

public class GetSubCategoryQueryHandler : IRequestHandler<GetSubCategoryQuery, ErrorOr<SubCategory>>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly ISubCategoriesRepository _subCategoriesRepository;

    public GetSubCategoryQueryHandler(ICategoriesRepository categoriesRepository, ISubCategoriesRepository subCategoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
        _subCategoriesRepository = subCategoriesRepository;
    }
    public async Task<ErrorOr<SubCategory>> Handle(GetSubCategoryQuery request, CancellationToken cancellationToken)
    {
        if (!await _categoriesRepository.ExistsAsync(request.CategoryId))
            return Error.NotFound(description: "Category not found.");

        if(await _subCategoriesRepository.GetByIdAsync(request.SubCategoryId) is not SubCategory subCategory)
        {
            return Error.NotFound(description: "Sub-category not found.");
        }

        return subCategory;
    }
}
