using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.HiddenVacancies.Queries;
using MRA.Jobs.Application.Contracts.HiddenVacancies.Queries.GetApplicationWithHiddenVacancy;

namespace MRA.Jobs.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HiddenVacanciesController : ApiControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetFirst()
        {
            var response = await Mediator.Send(new GetFirstHiddenVacancyQuery());
            return Ok(response);
        }

        [HttpGet("GetApplicationStatus")]
        public async Task<IActionResult> GetApplicationStatus()
        {
            var response = await Mediator.Send(new GetStatusApplicationWithHiddenVacancyQuery());
            return Ok(response);
        }
    }
}