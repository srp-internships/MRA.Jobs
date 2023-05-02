﻿using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

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
    public async Task<ActionResult<long>> Update([FromRoute] long id, [FromBody] UpdateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest();

        return await Mediator.Send(request, cancellationToken);
    }
}
