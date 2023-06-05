using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
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
    public async Task<IActionResult> Get([FromQuery] PaggedListQuery<JobVacancyListDTO> query)
    {
        var categories = await Mediator.Send(query);
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var category = await Mediator.Send(new GetJobVacancyByIdQuery { Id = id });
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewJobVacancy([FromBody] CreateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{id}/test")]
    public async Task<ActionResult<TestInfoDTO>> SendTestCreationRequest([FromRoute] Guid id, [FromBody] CreateTestCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{id}/test/result")]
    public async Task<ActionResult<TestResultDTO>> GetTestResultRequest([FromRoute] Guid id, [FromBody] CreateTestResultCommand request, CancellationToken cancellationToken)
    {
        if (id != request.TestId)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> Update([FromRoute] Guid id, [FromBody] UpdateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("{id}/tags")]
    public async Task<IActionResult> AddTag([FromRoute] Guid id, [FromBody] AddTagsToJobVacancyCommand request, CancellationToken cancellationToken)
    {
        request.JobVacancyId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}/tags")]
    public async Task<IActionResult> RemoveTags([FromRoute] Guid id, [FromBody] RemoveTagsFromJobVacancyCommand request, CancellationToken cancellationToken)
    {
        request.JobVacancyId = id;
        await Mediator.Send(request, cancellationToken);
        return Ok();
    }
}
