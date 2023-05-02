using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRA_Jobs.Application.Common.Interfaces;
using MRA_Jobs.Application.Common.Models;
using MRA_Jobs.Application.Common.Models.Dtos.ApplicationDtos;

namespace MRA_Jobs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;

        public ApplicationController(IApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var applications = await _service.GetAll<GetApplicationDto>();
            return Ok(applications);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(long id)
        {
            var application = await _service.GetById<GetApplicationDto>(id);
            return Ok(application);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateApplicationDto update_application)
        {
            var application = await _service.Update<UpdateApplicationDto, GetApplicationDto>(update_application);
            return Ok(application);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddApplicationDto add_application)
        {
            var application = await _service.Add<AddApplicationDto, GetApplicationDto>(add_application);
            return Ok(application);
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
    }
}
