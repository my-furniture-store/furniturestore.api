using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Commands.DeleteSubCategory;

public class DeleteSubCategoryCommandHandler : IRequestHandler<DeleteSubCategoryCommand, ErrorOr<Deleted>>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly ISubCategoriesRepository _subCategoriesRepository;
    private readonly IUnitofWork _unitofWork;

    public DeleteSubCategoryCommandHandler(ICategoriesRepository categoriesRepository, ISubCategoriesRepository subCategoriesRepository, IUnitofWork unitofWork)
    {
        _categoriesRepository = categoriesRepository;
        _subCategoriesRepository = subCategoriesRepository;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoriesRepository.GetByIdAsync(request.SubCategoryId);

        if(subCategory is null)
        {
            return Error.NotFound(description: "Sub-category not found.");
        }

        var category = await _categoriesRepository.GetByIdAsync(request.CategoryId);

        if (category is null)
        {
            return Error.NotFound(description: "Category not found.");
        }

        if(!category.HasSubCategory(request.SubCategoryId))
        {
            return Error.NotFound(description: "Sub-category not found.");
        }

        await _subCategoriesRepository.RemoveSubCategoryAsync(subCategory);
        await _unitofWork.CommitChangesAsync();

        return Result.Deleted;
    }
}
