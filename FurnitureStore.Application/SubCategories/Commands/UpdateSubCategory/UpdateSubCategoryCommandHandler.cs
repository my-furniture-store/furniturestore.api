using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.SubCategories;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Commands.UpdateSubCategory;

public class UpdateSubCategoryCommandHandler : IRequestHandler<UpdateSubCategoryCommand, ErrorOr<SubCategory>>
{
    private readonly ISubCategoriesRepository _subCategoriesRepository;
    private readonly IUnitofWork _unitofWork;

    public UpdateSubCategoryCommandHandler(ISubCategoriesRepository subCategoriesRepository, IUnitofWork unitofWork)
    {
        _subCategoriesRepository = subCategoriesRepository;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<SubCategory>> Handle(UpdateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoriesRepository.GetByIdAsync(request.SubCategoryId);

        if (subCategory is null)
            return Error.NotFound(description: "Sub-category not found.");

        subCategory.UpdateSubCategory(request.NewName);

        await _subCategoriesRepository.UpdateAsync(subCategory);
        await _unitofWork.CommitChangesAsync();

        return subCategory;
    }
}
