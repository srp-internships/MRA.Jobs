using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Common.Models;

namespace MRA.Jobs.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmailTestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailTestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] EmailMessage emailMessage)
    {
        await _emailService.SendEmailAsync(emailMessage);
        return Ok();
    }
}
