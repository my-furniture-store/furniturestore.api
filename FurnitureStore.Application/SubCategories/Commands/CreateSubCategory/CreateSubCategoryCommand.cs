using ErrorOr;
using FurnitureStore.Domain.SubCategories;
using MediatR;

namespace FurnitureStore.Application.SubCategories.Commands.CreateSubCategory;

public  record CreateSubCategoryCommand(string Name, Guid CategoryId) : IRequest<ErrorOr<SubCategory>>;

