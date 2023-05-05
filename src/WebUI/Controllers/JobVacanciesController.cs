using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Web.Controllers;

public class JobVacanciesController : ApiControllerBase
{
    private readonly ILogger<OidcConfigurationController> _logger;

    public JobVacanciesController(ILogger<OidcConfigurationController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreaeNewJobVacancy(CreateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<long>> UpdateJobVacancy([FromRoute] long id, [FromBody] UpdateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet]
    public async Task<ActionResult<List<JobVacancyResponse>>> GetJobVacancies(GetJobVacanciesQuery request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobVacancyFullResponse>> GetJobVacancyById([FromRoute] long id, GetJobVacancyByIdQuery request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }
}
