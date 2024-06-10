using FurnitureStore.API.Utils;
using FurnitureStore.Application.Products.Commands.CreateProduct;
using FurnitureStore.Application.Products.Commands.UpdateProduct;
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

    [HttpPut("{productId:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid productId, UpdateProductRequest request)
    {
        var command = new UpdateProductCommand(
            ProductId: productId,
            Name: request.Name,
            Price: request.Price,
            IsFeatured: request.IsFeatured,
            Description: request.Description,
            SKU: request.SKU,
            StockQuantity: request.StockQuantity,
            ImageUrl: request.ImageUrl,
            Dimensions: request.Dimensions,
            Weight: request.Weight,
            Material: request.Material,
            Colors: request.Colors?.ConvertAll(ProductHelper.ToDomainType),
            Brand: request.Brand,
            Rating: request.Rating,
            Discount: request.Discount,
            ProductStatus: ProductHelper.ToDomainType(request.ProductStatus));

        var updateProductResult = await _mediator.Send(command);

        return updateProductResult.MatchFirst(
            product => Ok(ProductHelper.ToDto(product)),
            errors => Problem(errors));
    }

}
