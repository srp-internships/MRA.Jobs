using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace MRA_Jobs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _service.GetAll<GetCategoryDto>();
            return Ok(categories);
         
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var category = await _service.GetById<GetCategoryDto>(id);
            return Ok(category);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto update_category)
        {
            var category =await _service.Update<UpdateCategoryDto, GetCategoryDto>(update_category);
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryDto add_category)
        {
            var category = await _service.Add<AddCategoryDto, GetCategoryDto>(add_category);
            return Ok(category);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete( long id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
    }
}
