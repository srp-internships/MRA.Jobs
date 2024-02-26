using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.DeleteJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobs;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Infrastructure.Identity;

namespace MRA.Jobs.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(ApplicationPolicies.Reviewer)]
public class JobsController : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] GetJobsQueryOptions query)
    {
        var categories = await Mediator.Send(query);
        return Ok(categories);
    }
    

    [HttpGet("{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromRoute] string slug)
    {
        var category = await Mediator.Send(new GetJobVacancyBySlugQuery { Slug = slug });
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<JSObject>> CreateNewJobVacancy([FromBody] CreateJobVacancyCommand request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { slug = result }, result);
    }

    [HttpPut("{slug}")]
    public async Task<ActionResult<string>> Update([FromRoute] string slug, [FromBody] UpdateJobVacancyCommand request,
        CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        var result = await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { slug = result }, result);
    }

    [HttpDelete("{slug}")]
    public async Task<ActionResult<bool>> DeleteJobVacancy([FromRoute] string slug, CancellationToken cancellationToken)
    {
        var request = new DeleteJobVacancyCommand { Slug = slug };
        return await Mediator.Send(request, cancellationToken);
    }
}