using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.SubCategories;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Commands.CreateSubCategory;

public class CreateSubCategoryCommandHandler : IRequestHandler<CreateSubCategoryCommand, ErrorOr<SubCategory>>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly ISubCategoriesRepository _subCategoriesRepository;
    private readonly IUnitofWork _unitofWork;

    public CreateSubCategoryCommandHandler(ICategoriesRepository categoriesRepository, ISubCategoriesRepository subCategoriesRepository, IUnitofWork unitofWork)
    {
        _categoriesRepository = categoriesRepository;
        _subCategoriesRepository = subCategoriesRepository;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<SubCategory>> Handle(CreateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoriesRepository.GetByIdAsync(request.CategoryId);

        if (category is null)
        {
            return Error.NotFound(description: "Category not found.");
        }

        var subCategory = new SubCategory(request.Name, request.CategoryId);

        var addSubCategoryResult = category.AddSubCategory(subCategory);

        if(addSubCategoryResult.IsError)
        {
            return addSubCategoryResult.Errors;
        }

        await _subCategoriesRepository.AddSubCategoryAsync(subCategory);
        await _unitofWork.CommitChangesAsync();

        return subCategory;
    }
}
