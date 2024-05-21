using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.SubCategories;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Queries.ListSubCategories;

public class ListSubCategoriesQueryHandler : IRequestHandler<ListSubCategoriesQuery, ErrorOr<List<SubCategory>>>
{
    private readonly ICategoriesRepository _categoriesRepository;                                       
    private readonly ISubCategoriesRepository _subCategoriesRepository;

    public ListSubCategoriesQueryHandler(ICategoriesRepository categoriesRepository, ISubCategoriesRepository subCategoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
        _subCategoriesRepository = subCategoriesRepository;
    }

    public async Task<ErrorOr<List<SubCategory>>> Handle(ListSubCategoriesQuery request, CancellationToken cancellationToken)
    {
        if(!await _categoriesRepository.ExistsAsync(request.CategoryId))
        {
            return Error.NotFound(description: "Category not found.");
        }

        return await _subCategoriesRepository.ListByCategoryIdAsync(request.CategoryId);
    }
}
