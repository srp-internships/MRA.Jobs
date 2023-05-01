using Microsoft.AspNetCore.Mvc;
using MRA_Jobs.Application.Abstractions;
using MRA_Jobs.Domain.Entities;

namespace MRAJobs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IEntityService<Vacancy> _service;

        public ValuesController(IEntityService<Vacancy> service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }
        
    }
}
