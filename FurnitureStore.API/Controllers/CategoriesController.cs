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
}
