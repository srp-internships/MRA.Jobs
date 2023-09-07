using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.Claim.Commands;
using MRA.Identity.Application.Contract.Claim.Queries;
using MRA.Identity.Application.Contract.Claim.Responses;

namespace MRA.Identity.Api.Controllers;

[ApiController]
[Route("/api/[controller]/")]
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
        ApplicationResponse<Guid> response = await _mediator.Send(request);
        if (response.IsSuccess)
        {
            return Ok();
        }

        return BadRequest($"{response.Exception}   {response.ErrorMessage}");
    }


    [HttpPut("{slug}")]
    public async Task<IActionResult> Put([FromBody] UpdateClaimCommand command, string slug)
    {

        if (slug!=command.Slug)
        {
            return BadRequest();
        }
        ApplicationResponse response = await _mediator.Send(command);

        if (response.IsSuccess)
        {
            return Ok();
        }

        return BadRequest($"{response.Exception}   {response.ErrorMessage}");

    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        var response = await _mediator.Send(new DeleteClaimCommand
        {
            Slug = slug
        }); 
        
        if (response.IsSuccess)
        {
            return Ok();
        }
        
        return BadRequest($"{response.Exception}   {response.ErrorMessage}");
    }
    
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllQuery query)
    {
        
        ApplicationResponse<List<UserClaimsResponse>> response = await _mediator.Send(query);
        
        if (response.IsSuccess)
        {
            return Ok(response.Response);
        }
        
        return BadRequest($"{response.Exception}   {response.ErrorMessage}");
    }    
    
    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {

        var response = await _mediator.Send(new GetBySlugQuery { Slug = slug });
        
        if (response.IsSuccess)
        {
            return Ok(response.Response);
        }
        
        return BadRequest($"{response.Exception}   {response.ErrorMessage}");
    }
}