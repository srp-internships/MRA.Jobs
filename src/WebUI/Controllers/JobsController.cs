using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.Tests.Commands;

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
        PagedList<JobVacancyListDto> categories = await Mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        JobVacancyDetailsDto category = await Mediator.Send(new GetJobVacancyByIdQuery { Id = id });
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewJobVacancy([FromBody] CreateJobVacancyCommand request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{id}/test")]
    public async Task<ActionResult<TestInfoDto>> SendTestCreationRequest([FromRoute] Guid id,
        [FromBody] CreateTestCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{id}/test/result")]
    public async Task<ActionResult<TestResultDto>> GetTestResultRequest([FromRoute] Guid id,
        [FromBody] CreateTestResultCommand request, CancellationToken cancellationToken)
    {
        if (id != request.TestId)
        {
            return BadRequest();
        }

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> Update([FromRoute] Guid id, [FromBody] UpdateJobVacancyCommand request,
        CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteJobVacancy([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        DeleteJobVacancyCommand request = new DeleteJobVacancyCommand { Id = id };
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{id}/tags")]
    public async Task<IActionResult> AddTag([FromRoute] Guid id, [FromBody] AddTagsToJobVacancyCommand request,
        CancellationToken cancellationToken)
    {
        request.JobVacancyId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}/tags")]
    public async Task<IActionResult> RemoveTags([FromRoute] Guid id, [FromBody] RemoveTagsFromJobVacancyCommand request,
        CancellationToken cancellationToken)
    {
        request.JobVacancyId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }
}