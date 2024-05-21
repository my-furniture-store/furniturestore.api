using FurnitureStore.Application.SubCategories.Commands.CreateSubCategory;
using FurnitureStore.Application.SubCategories.Commands.DeleteSubCategory;
using FurnitureStore.Application.SubCategories.Commands.UpdateSubCategory;
using FurnitureStore.Application.SubCategories.Queries.GetSubCategory;
using FurnitureStore.Application.SubCategories.Queries.ListSubCategories;
using FurnitureStore.Contracts.SubCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Controllers;

[Route("api/categories/{categoryId:guid}/[controller]")]
public class SubCategoriesController : ApiController
{
    private readonly ISender _mediator;

    public SubCategoriesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubCategory(CreateSubCategoryRequest request, Guid categoryId)
    {
        var command = new CreateSubCategoryCommand(request.Name, categoryId);

        var createSubCategoryResult = await _mediator.Send(command);

        return createSubCategoryResult.Match(
            subCategory =>
                    CreatedAtAction(
                        nameof(GetSubCategory),
                        new { categoryId, SubCategoryId = subCategory.Id },
                        new SubCategoryResponse(subCategory.Id, subCategory.Name)
                        ),
            errors => Problem(errors)
            );
    }

    [HttpGet("{subCategoryId:guid}")]
    public async Task<IActionResult> GetSubCategory(Guid categoryId, Guid subCategoryId)
    {
        var query = new GetSubCategoryQuery(subCategoryId, categoryId);

        var getSubCategoryResult = await _mediator.Send(query);

        return getSubCategoryResult.Match(
            subCategory => Ok(new SubCategoryResponse(subCategory.Id, subCategory.Name)),
            errors => Problem(errors));
    }

    [HttpGet]
    public async Task<IActionResult> ListSubCategories(Guid categoryId)
    {
        var query = new ListSubCategoriesQuery(categoryId);

        var listSubCategoriesResult = await _mediator.Send(query);

        return listSubCategoriesResult.Match(
            subCategories => Ok(subCategories.ConvertAll(subCategory => new SubCategoryResponse(subCategory.Id, subCategory.Name))),
            errors => Problem(errors));
    }

    [HttpPut("{subCategoryId:guid}")]
    public async Task<IActionResult> UpdateSubCategory(UpdateSubCategoryRequest request, Guid categoryId, Guid subCategoryId)
    {
        var command = new UpdateSubCategoryCommand(
            CategoryId: categoryId,
            SubCategoryId: subCategoryId,
            NewName: request.Name);

        var updateSubCategoryResult = await _mediator.Send(command);

        return updateSubCategoryResult.Match(
            subCategory => Ok(new SubCategoryResponse(subCategory.Id, subCategory.Name)),
            errors => Problem(errors));
    }

    [HttpDelete("{subCategoryId:guid}")]
    public async Task<IActionResult> DeleteSubCategory(Guid categoryId, Guid subCategoryId)
    {
        var command = new DeleteSubCategoryCommand(categoryId, subCategoryId);

        var deleteSubCategoryResult = await _mediator.Send(command);

        return deleteSubCategoryResult.Match<IActionResult>(_ => NoContent(), errors => Problem(errors));
    }
}
