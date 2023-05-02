using Microsoft.AspNetCore.Mvc;
using MRA_Jobs.Application.Common.Interfaces;
using MRA_Jobs.Domain.Entities;

namespace MRAJobs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobVacanciesController : ControllerBase
    {
        IJobVacancyService _jobVacancyService;

        public JobVacanciesController(IJobVacancyService jobVacancyService)
        {
            _jobVacancyService = jobVacancyService;
        }


        // GET: api/JobVacancies
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobVacancy>>> GetJobVacancies()
        {
            throw new NotImplementedException();
        }

        // GET: api/JobVacancies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobVacancy>> GetJobVacancy(long id)
        {
            throw new NotImplementedException();
        }

        // PUT: api/JobVacancies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobVacancy(long id, JobVacancy jobVacancy)
        {
            throw new NotImplementedException();
        }

        // POST: api/JobVacancies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobVacancy>> PostJobVacancy(JobVacancy jobVacancy)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/JobVacancies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobVacancy(long id)
        {
            throw new NotImplementedException();
        }

        private bool JobVacancyExists(long id)
        {
            //return (_context.JobVacancies?.Any(e => e.Id == id)).GetValueOrDefault();
            throw new NotImplementedException();
        }
    }
}
