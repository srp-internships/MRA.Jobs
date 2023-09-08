using MediatR;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.User.Queries;

namespace MRA.Identity.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SmsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SmsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("send_sms")]
    public async Task<IActionResult> SendSms([FromQuery] SendSmsQuery request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
