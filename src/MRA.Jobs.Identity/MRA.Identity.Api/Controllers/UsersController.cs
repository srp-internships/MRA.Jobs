using MediatR;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.User.Queries;

namespace MRA.Identity.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]GetAllUsersQuery query)
    {
        var users = await _mediator.Send(query);
        return Ok(users);
    }
    [HttpGet("{userName}")]
    public async Task<IActionResult> Get(string userName)
    {
        var query = new GetUserByUsernameQuery { UserName = userName };
        var user = await _mediator.Send(query);
        return Ok(user);
    }


}
