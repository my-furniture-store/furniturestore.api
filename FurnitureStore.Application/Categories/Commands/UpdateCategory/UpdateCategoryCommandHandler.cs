using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Categories;
using MediatR;

namespace FurnitureStore.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ErrorOr<Category>>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IUnitofWork _unitofWork;

    public UpdateCategoryCommandHandler(ICategoriesRepository categoriesRepository, IUnitofWork unitofWork)
    {
        _categoriesRepository = categoriesRepository;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<Category>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryToUpdate = await _categoriesRepository.GetByIdAsync(request.CategoryId);

        if (categoryToUpdate is null)
        {
            return Error.NotFound(description: "Category not found.");
        }

        var category = new Category
        (
            name: categoryToUpdate.Name,
            id: categoryToUpdate.Id
        );

        await _categoriesRepository.UpdateCategoryAsync(category);
        await _unitofWork.CommitChangesAsync();

        return category;
    }
}
