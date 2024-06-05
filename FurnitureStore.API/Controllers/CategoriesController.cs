using FurnitureStore.API.Utils;
using FurnitureStore.Application.Categories.Commands.CreateCategory;
using FurnitureStore.Application.Categories.Commands.DeleteCategory;
using FurnitureStore.Application.Categories.Commands.UpdateCategory;
using FurnitureStore.Application.Categories.Queries.GetCategory;
using FurnitureStore.Application.Categories.Queries.ListCategories;
using FurnitureStore.Application.Products.Queries.ListCategoryProducts;
using FurnitureStore.Contracts.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Controllers;

[Route("api/[controller]")]
public class CategoriesController : ApiController
{
    private readonly ISender _mediator;

    public CategoriesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> ListCategories()
    {
        var query = new ListCategoriesQuery();

        var listCategoriesResult = await _mediator.Send(query);

        return listCategoriesResult.Match(
            categories => Ok(categories.ConvertAll(category => new CategoryResponse(category.Id, category.Name))),
            errors => Problem(errors)
            );
    }

    [HttpGet("{categoryId:guid}")]
    public async Task<IActionResult> GetCategory(Guid categoryId)
    {
        var query = new GetCategoryQuery(categoryId);

        var getCategoryResult = await _mediator.Send(query);

        return getCategoryResult.MatchFirst(
            category =>
            Ok(new CategoryResponse(categoryId, category.Name)),
            errors => Problem(errors)
            );
    }

    [HttpGet("{categoryId:guid}/products")]
    public async Task<IActionResult> GetProductsByCategoryId(Guid categoryId)
    {
        var query = new ListCategoryProductsQuery(categoryId);
        
        var productsResult = await _mediator.Send(query);

        return productsResult.Match(
            products => Ok(products.ConvertAll(product => ProductHelper.ToDto(product))),
            errors => Problem(errors)
            );
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequest categoryRequest)
    {
        var command = new CreateCategoryCommand(categoryRequest.Name);

        var createCategoryResult = await _mediator.Send(command);

        return createCategoryResult.MatchFirst(
            category => CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, new CategoryResponse(category.Id, category.Name)),
            errors => Problem(errors)
            );
    }

    [HttpPut("{categoryId:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid categoryId, UpdateCategoryRequest categoryRequest)
    {
        var command = new UpdateCategoryCommand(categoryId, categoryRequest.Name);

        var updateCategoryResult = await _mediator.Send(command);

        return updateCategoryResult.MatchFirst(
            category => Ok(new CategoryResponse(categoryId, category.Name)),
            errors => Problem(errors)
            );
    }

    [HttpDelete("{categoryId:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid categoryId)
    {
        var command = new DeleteCategoryCommand(categoryId);

        var deleteCategoryResult = await _mediator.Send(command);

        return deleteCategoryResult.Match<IActionResult>(_ => NoContent(), errors => Problem(errors));
    }


}
