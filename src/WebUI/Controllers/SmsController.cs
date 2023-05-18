using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.SMSService;
using MRA.Jobs.Infrastructure.Services;

namespace MRA.Jobs.Web.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SmsController : ApiControllerBase
{
    private readonly ISmsService _smsService;

    public SmsController(ISmsService smsService)
    {
        _smsService = smsService;
    }

    [HttpPost]
    public ActionResult<string> Create(SmsMessage message)
    {
        return Ok(_smsService.SendSmsAsync(message));
    }
}