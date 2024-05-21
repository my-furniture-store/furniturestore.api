using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using MediatR;

namespace FurnitureStore.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ErrorOr<Deleted>>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IUnitofWork _unitofWork;

    public DeleteCategoryCommandHandler(ICategoriesRepository categoriesRepository, IUnitofWork unitofWork)
    {
        _categoriesRepository = categoriesRepository;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoriesRepository.GetByIdAsync(request.CategoryId);

        if(category is null) 
        {
            return Error.NotFound(description: "Category not found.");
        }

        if(category.SubCategories!.Any())
        {
            return Error.Conflict(description: "Can't delete category with associated sub-categories.");
        }

        await _categoriesRepository.RemoveCategoryAsync(category);
        await _unitofWork.CommitChangesAsync();

        return Result.Deleted;
    }
}
