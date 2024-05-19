using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Categories;
using MediatR;

namespace FurnitureStore.Application.Categories.Queries.GetCategory;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, ErrorOr<Category>>
{
    private readonly ICategoriesRepository _categoriesRepository;

    public GetCategoryQueryHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<ErrorOr<Category>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        if (!await _categoriesRepository.ExistsAsync(request.CategoryId))
        {
            return Error.NotFound(description:"Category not found.");
        }

        var category = await _categoriesRepository.GetByIdAsync(request.CategoryId);

        return category!;
    }
}
