using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Tests.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TrainingsController : ApiControllerBase
{

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var training = await Mediator.Send(new GetTrainingVacancyBySlugQuery { Slug = slug });
        return Ok(training);
    }
    [HttpGet]
    public async Task<ActionResult<PagedList<TrainingVacancyListDto>>> Get([FromQuery] GetTrainingsQueryOptions query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<string>> Post(CreateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { slug = result }, result);
    }

    [HttpPut("{slug}")]
    public async Task<ActionResult<string>> Put([FromRoute] string slug, [FromBody] UpdateTrainingVacancyCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return NotFound();

        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { slug = result }, result);
    }

    [HttpDelete("{slug}")]
    public async Task<ActionResult<bool>> Delete([FromRoute] string slug, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new DeleteTrainingVacancyCommand { Slug = slug }, cancellationToken);
    }






}
