// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using MRA.Jobs.Application.Contracts.NoVacancies.Queries;
// using MRA.Jobs.Application.Contracts.NoVacancies.Queries.GetApplicationWithNoVacancy;
//
// namespace MRA.Jobs.Web.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     [Authorize]
//     public class NoVacanciesController : ApiControllerBase
//     {
//         [HttpGet]
//         [AllowAnonymous]
//         public async Task<IActionResult> GetFirst()
//         {
//             var response = await Mediator.Send(new GetFirstNoVacancyQuery());
//             return Ok(response);
//         }
//
//         [HttpGet("GetApplicationStatus")]
//         public async Task<IActionResult> GetApplicationStatus()
//         {
//             var response = await Mediator.Send(new GetStatusApplicationWithNoVacancyQuery());
//             return Ok(response);
//         }
//     }
// }