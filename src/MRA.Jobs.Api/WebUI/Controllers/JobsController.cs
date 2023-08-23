using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.Tests.Commands;
using AddTagsToJobVacancyCommand = MRA.Jobs.Application.Contracts.JobVacancies.Commands.AddTagsToJobVacancyCommand;

namespace MRA.Jobs.Web.Controllers;


[ApiController]
[Route("api/[controller]")]
public class JobsController : ApiControllerBase
{


    private readonly ILogger<JobsController> _logger;

    public JobsController(ILogger<JobsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PagedListQuery<JobVacancyListDto> query)
    {
        var categories = await Mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get([FromRoute] string slug)
    {
        var category = await Mediator.Send(new GetJobVacancyBySlugQuery { Slug = slug });
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewJobVacancy([FromBody] CreateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{slug}/test")]
    public async Task<ActionResult<TestInfoDto>> SendTestCreationRequest([FromRoute] string slug, [FromBody] CreateTestCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{slug}/test/result")]
    public async Task<ActionResult<TestResultDto>> GetTestResultRequest([FromRoute] string slug, [FromBody] CreateTestResultCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{slug}")]
    public async Task<ActionResult<Guid>> Update([FromRoute] string slug, [FromBody] UpdateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{slug}")]
    public async Task<ActionResult<bool>> DeleteJobVacancy([FromRoute] string slug, CancellationToken cancellationToken)
    {
        var request = new DeleteJobVacancyCommand { Slug = slug };
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{slug}/tags")]
    public async Task<IActionResult> AddTag([FromRoute] string slug, [FromBody] AddTagsToJobVacancyCommand request, CancellationToken cancellationToken)
    {
        request.JobVacancySlug = slug;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{slug}/tags")]
    public async Task<IActionResult> RemoveTags([FromRoute] string slug, [FromBody] RemoveTagsFromJobVacancyCommand request, CancellationToken cancellationToken)
    {
        request.JobVacancySlug = slug;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }
}
