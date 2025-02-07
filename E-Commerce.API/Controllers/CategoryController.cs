using E_Commerce.Core.DTO;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
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
        public async Task<Response<CategoryDTO>> AddCategory(CategoryDTO dto)
        {
            return await _categoryService.AddCategory(dto);
        }

        [HttpGet]
        [Route(nameof(GetCategories))]
        public async Task<Response<IEnumerable<GetCategoryDTO>>> GetCategories()
        {
            return await _categoryService.GetCategories();
        }

        [HttpGet]
        [Route(nameof(GetCategory))]
        public async Task<Response<GetCategoryDTO>> GetCategory(int id)
        {
            return await _categoryService.GetCategory(id);
           
        }
    }
}
