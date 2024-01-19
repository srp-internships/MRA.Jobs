using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.Messages.Commands;
using MRA.Identity.Application.Contract.Messages.Queries;

namespace MRA.Identity.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(ApplicationPolicies.Administrator)]
public class MessageController : ControllerBase
{
    private readonly ISender _mediator;

    public MessageController(ISender mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> SendMessage(SendMessageCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetMessages()
    {
        GetAllMessagesQuery query = new();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
