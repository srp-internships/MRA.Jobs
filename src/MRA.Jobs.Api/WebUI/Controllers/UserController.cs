using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.User.Commands.UsersByApplications;
using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Infrastructure.Identity;

namespace MRA.Jobs.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(ApplicationPolicies.Reviewer)]
    public class UserController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPagedListUsersQuery query)
        {
            var httpClient = httpClientFactory.CreateClient("mra-jobs");
            var command = new GetPagedListUsersCommand()
            {
                ApplicationId = Guid.Parse(configuration["Application:Id"]),
                ApplicationClientSecret = configuration["Application:ClientSecret"],
                Filters = query.Filters,
                Skills = query.Skills,
                Sorts = query.Sorts,
                PageSize = query.PageSize,
                Page = query.Page
            };
            var response = await httpClient.PostAsJsonAsync(
                configuration["MraJobs-IdentityApi:Users"],
                command);
            var result = await response.Content.ReadFromJsonAsync<PagedList<UserResponse>>();
            return Ok(result);
        }
    }
}