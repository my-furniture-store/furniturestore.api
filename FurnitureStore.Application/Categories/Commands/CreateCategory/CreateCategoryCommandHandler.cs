using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Categories;
using MediatR;

namespace FurnitureStore.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ErrorOr<Category>>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IUnitofWork _unitofWork;

    public CreateCategoryCommandHandler(ICategoriesRepository categoriesRepository, IUnitofWork unitofWork)
    {
        _categoriesRepository = categoriesRepository;
        _unitofWork = unitofWork;
    }

    public async Task<ErrorOr<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Name);

        await _categoriesRepository.AddCategoryAsync(category);
        await _unitofWork.CommitChangesAsync();

        return category;
    }
}
