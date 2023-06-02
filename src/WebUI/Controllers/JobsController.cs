﻿using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Features.JobVacancies.Commands.Tags;
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

    [HttpGet("{title}")]
    public async Task<IActionResult> GetByTitle([FromRoute] string title)
    {
        var result = await Mediator.Send(title);
        return Ok(result);
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
