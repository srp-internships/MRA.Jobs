using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternships;
using MRA.Jobs.Infrastructure.Identity;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(ApplicationPolicies.Reviewer)]
public class InternshipsController : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetWithPagination([FromQuery] GetInternshipsQueryOptions query)
    {
        var internships = await Mediator.Send(query);
        return Ok(internships);
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateNewInternship(CreateInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        var result= await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetInternshipBySlug), new { slug = result }, result);
    }

    [HttpPut("{slug}")]
    public async Task<ActionResult<string>> UpdateInternship([FromRoute] string slug, [FromBody] UpdateInternshipVacancyCommand request, CancellationToken cancellationToken)
    {
        if (slug != request.Slug)
            return BadRequest();

        var result= await Mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetInternshipBySlug), new { slug = result }, result);
    }

    [HttpDelete("{slug}")]
    public async Task<ActionResult<bool>> DeleteInternship([FromRoute] string slug, CancellationToken cancellationToken)
    {
        var request = new DeleteInternshipVacancyCommand { Slug = slug };
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet("{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetInternshipBySlug(string slug)
    {
        var internship = await Mediator.Send(new GetInternshipVacancyBySlugQuery { Slug = slug });
        return Ok(internship);
    }
}
