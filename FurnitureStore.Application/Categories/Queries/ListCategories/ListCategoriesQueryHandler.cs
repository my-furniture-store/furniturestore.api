using ErrorOr;
using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Categories;
using MediatR;

namespace FurnitureStore.Application.Categories.Queries.ListCategories;

public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, ErrorOr<List<Category>>>
{
    private readonly ICategoriesRepository _categoriesRepository;

    public ListCategoriesQueryHandler(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<ErrorOr<List<Category>>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories =  await _categoriesRepository.GetAllCategoriesAsync();

        return categories;
    }
}
