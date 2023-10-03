
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Infrastructure.Identity;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.CreateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.UpdateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries.GetVacancyCategoryWithPagination;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.DeleteVacancyCategory;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobCategories;

namespace MRA.Jobs.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Application.Common.Security.Authorize]
public class CategoriesController : ApiControllerBase
{
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ILogger<CategoriesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<CategoryResponse>>> GetAll(
        [FromQuery] PagedListQuery<CategoryResponse> query)
    {
        var categories = await Mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("training")]
    public async Task<IActionResult> GetCategories([FromQuery] GetTrainingCategoriesQuery query)
    {
        var categories = await Mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("job")]
    public async Task<IActionResult> GetCategories([FromQuery] GetJobCategoriesQuery query)
    {
        var categories = await Mediator.Send(query);
        return Ok(categories);
    } 

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var category = await Mediator.Send(new GetVacancyCategoryBySlugQuery { Slug = slug });
        return Ok(category);
    }

    [HttpPost]
    [Authorize(ApplicationPolicies.Reviewer)]
    public async Task<ActionResult<string>> CreateNewCategoryVacancy(CreateVacancyCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { slug = result }, result);
    }


    [HttpPut("{slug}")]
    [Authorize(ApplicationPolicies.Reviewer)]
    public async Task<ActionResult<string>> Update([FromRoute] string slug,
        [FromBody] UpdateVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { slug = result }, result);
    }

    [HttpDelete("{slug}")]
    [Authorize(ApplicationPolicies.Reviewer)]
    public async Task<IActionResult> Delete(string slug)
    {
        var command = new DeleteVacancyCategoryCommand { Slug = slug };
        await Mediator.Send(command);
        return NoContent();
    }
}