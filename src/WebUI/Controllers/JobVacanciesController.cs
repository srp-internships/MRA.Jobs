using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Web.Controllers;


[ApiController]
[Route("api/[controller]")]
public class JobVacanciesController : ApiControllerBase
{
    private readonly ILogger<JobVacanciesController> _logger;

    public JobVacanciesController(ILogger<JobVacanciesController> logger)
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
    public async Task<IActionResult> Get(Guid id)
    {
        var category = await Mediator.Send(new GetJobVacancyByIdQuery { Id = id });
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateNewJobVacancy(CreateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> Update([FromRoute] Guid id, [FromBody] UpdateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }
}
