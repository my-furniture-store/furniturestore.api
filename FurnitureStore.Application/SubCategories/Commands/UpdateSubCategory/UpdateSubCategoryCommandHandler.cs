using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.SubCategories;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Commands.UpdateSubCategory;

public class UpdateSubCategoryCommandHandler : IRequestHandler<UpdateSubCategoryCommand, ErrorOr<SubCategory>>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly ISubCategoriesRepository _subCategoriesRepository;
    private readonly IUnitofWork _unitofWork;

    public UpdateSubCategoryCommandHandler(ISubCategoriesRepository subCategoriesRepository, IUnitofWork unitofWork, ICategoriesRepository categoriesRepository)
    {
        _subCategoriesRepository = subCategoriesRepository;
        _unitofWork = unitofWork;
        _categoriesRepository = categoriesRepository;
    }

    public async Task<ErrorOr<SubCategory>> Handle(UpdateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoriesRepository.GetByIdAsync(request.SubCategoryId);

        if (subCategory is null)
            return Error.NotFound(description: "Sub-category not found.");

        var category = await _categoriesRepository.GetByIdAsync(request.CategoryId);

        if (category is null)
        {
            return Error.NotFound(description: "Category not found.");
        }

        if (!category.HasSubCategory(request.SubCategoryId))
        {
            return Error.NotFound(description: "Sub-category not found.");
        }

        subCategory.UpdateSubCategory(request.NewName);

        await _subCategoriesRepository.UpdateAsync(subCategory);
        await _unitofWork.CommitChangesAsync();

        return subCategory;
    }
}
