using ErrorOr;
using FurnitureStore.Domain.Categories;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Commands.CreateSubCategory;

public  record CreateSubCategoryCommand(string Name, Guid CategoryId) : IRequest<ErrorOr<SubCategory>>;

