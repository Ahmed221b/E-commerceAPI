using E_Commerce.Core.DTO;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route(nameof(AddCategory))]
        public async Task<IActionResult> AddCategory(CategoryDTO dto)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryService.AddCategory(dto);
                if (category != null)
                {
                    return Created();
                }
            }
                return BadRequest();
        }

        [HttpGet]
        [Route(nameof(GetCategories))]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategories();
            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }
    }
}
