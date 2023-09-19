using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Common.Models;

namespace MRA.Jobs.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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