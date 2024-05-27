using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Users;
using MRA.Jobs.Infrastructure.Identity;

namespace MRA.Jobs.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(ApplicationPolicies.Reviewer)]
public class UserController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetPagedListUsersQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("List")]
    public async Task<IActionResult> Get([FromQuery] GetListUsersQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}