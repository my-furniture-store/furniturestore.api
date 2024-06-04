using FurnitureStore.API.Utils;
using FurnitureStore.Application.Products.Commands.CreateProduct;
using FurnitureStore.Application.Products.Queries.GetProduct;
using FurnitureStore.Application.Products.Queries.ListProducts;
using FurnitureStore.Contracts.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Controllers;

[Route("api/[controller]")]
public class ProductsController : ApiController
{
    private readonly ISender _mediator;

    public ProductsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> ListProducts()
    {
        var query = new ListProductsQuery();

        var productsListResult = await _mediator.Send(query);

        return productsListResult.Match(
            products => Ok(products.ConvertAll(product => ProductHelper.ToDto(product))),
            errors => Problem(errors));
    }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        var query = new GetProductQuery(productId);

        var productResult = await _mediator.Send(query);

        return productResult.MatchFirst(
            product => Ok(ProductHelper.ToDto(product)),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var command = new CreateProductCommand(request.Name, request.Price, request.CategoryId, request.SubCategoryId, request.IsFeatured);

        var createProductResult = await _mediator.Send(command);

        return createProductResult.MatchFirst(
            product => CreatedAtAction(nameof(GetProduct), new {productId = product.Id}, ProductHelper.ToDto(product)),
            errors => Problem(errors));
    }

}
