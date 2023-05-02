using Microsoft.AspNetCore.Mvc;
using MRA_Jobs.Application.Common.Interfaces;

namespace MRAJobs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobVacanciesController : ControllerBase
    {
        private readonly IJobVacancyService _jobVacancyService;

        public JobVacanciesController(IJobVacancyService jobVacancyService)
        {
            _jobVacancyService = jobVacancyService;
        }


    }
}
