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
    public async Task<IActionResult> SendSms([FromQuery] SendSmsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("verify_code")]
    public async Task<IActionResult> VerifyCode([FromQuery] SmsVerificationCodeCheckQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
