using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ApiControllerBase
{
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ILogger<CategoriesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PaggedList<CategoryResponse>>> GetAll([FromQuery] PaggedListQuery<CategoryResponse> query)
    {
        var categories = await Mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await Mediator.Send(new GetVacancyCategoryByIdQuery { Id = id });
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewJobVacancy(CreateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> Update([FromRoute] Guid id, [FromBody] UpdateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteVacancyCategoryCommand { Id = id };
        await Mediator.Send(command);
        return NoContent();
    }
}
