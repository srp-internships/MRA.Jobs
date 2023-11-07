using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.Claim.Commands;
using MRA.Identity.Application.Contract.Claim.Queries;
using MRA.Identity.Application.Contract.Claim.Responses;

namespace MRA.Identity.Api.Controllers;

[ApiController]
[Route("/api/[controller]/")]
[Authorize(ApplicationPolicies.Administrator)]
public class ClaimsController : ControllerBase
{
    private readonly ISender _mediator;

    public ClaimsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClaimCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok();
    }


    [HttpPut("{slug}")]
    public IActionResult Put([FromBody] UpdateClaimCommand command, string slug)
    {

        if (slug != command.Slug)
        {
            return BadRequest();
        }
        var response = _mediator.Send(command);
        return Ok();

    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        var response = await _mediator.Send(new DeleteClaimCommand
        {
            Slug = slug
        });
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllQuery query)
    {

        List<UserClaimsResponse> response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {

        var response = await _mediator.Send(new GetBySlugQuery { Slug = slug });
        return Ok(response);
    }
}