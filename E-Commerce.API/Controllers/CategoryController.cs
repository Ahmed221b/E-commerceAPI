using E_Commerce.Core.Custom_Exceptions;
using E_Commerce.Core.DTO.Category;
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
        public async Task<ActionResult<Response<GetCategoryDTO>>> AddCategory(CategoryDTO dto)
        {
            var response = new Response<GetCategoryDTO>();
            try
            {
                var result = await _categoryService.AddCategory(dto);
                response.Data = result;
                return Ok(response);
            }
            catch(ConflictException c)
            {
                response.Errors.Add(new Error { Code = 409, Message = c.Message });
                return Conflict(response);
            }
            catch (Exception ex) {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened" + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response); // 500 Internal Server Error
            };
        }

        [HttpGet]
        [Route(nameof(GetCategories))]
        public async Task<ActionResult<Response<IEnumerable<GetCategoryListDTO>>>> GetCategories()
        {
            var response = new Response<IEnumerable<GetCategoryListDTO>>();
            try
            {
                var categories = await _categoryService.GetCategories();
                if (categories.Count() == 0)
                {
                    response.Errors.Add(new Error { Code = 404, Message = "Categories Not found" });
                    return NotFound(response);
                }
                response.Data = categories;
                return Ok(categories);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened" + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route(nameof(GetCategory))]
        public async Task<ActionResult<Response<GetCategoryDTO>>> GetCategory(int id)
        {
            var response = new Response<GetCategoryDTO>();
            try
            {
                var category = await _categoryService.GetCategory(id);
                if (category == null)
                {
                    response.Errors.Add(new Error { Code = 404, Message = "Category Not found" });
                    return NotFound(response);
                }
                response.Data = category;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened" + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response); // 500 Internal Server Erro
            }
        }

        [HttpPut]
        [Route(nameof(UpdateCategory))]
        public async Task<ActionResult<Response<CategoryDTO>>> UpdateCategory(int id, CategoryDTO dto)
        {
            var response = new Response<CategoryDTO>();
            try
            {
                var result = await _categoryService.UpdateCategory(id, dto);
                if (result == null)
                {
                    response.Errors.Add(new Error { Code = 404, Message = "Category Not found" });
                    return NotFound(response);
                }
                response.Data = result;
                return Ok(response);
            }
            catch(ConflictException ex)
            {
                response.Errors.Add(new Error { Code = 409, Message = ex.Message });
                return Conflict(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened" + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response); // 500 Internal Server Error
            }
        }

        [HttpPost]
        [Route(nameof(DeleteCategory))]
        public async Task<ActionResult<Response<string>>> DeleteCategory(int id)
        {
            var response = new Response<string>();
            try
            {
                var result = await _categoryService.DeleteCategory(id);
                if (!result)
                {
                    response.Errors.Add(new Error { Code = 404, Message = "Category Not found" });
                    return NotFound(response);
                }
                response.Data = "Category deleted successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened" + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route(nameof(SearchCategories))]
        public async Task<ActionResult<Response<IEnumerable<GetCategoryDTO>>>> SearchCategories(string name)
        {
            var response = new Response<IEnumerable<GetCategoryDTO>>();
            try
            {
                var categories = await _categoryService.SearchCategories(name);
                if (categories.Count() == 0)
                {
                    response.Errors.Add(new Error { Code = 404, Message = "No match found!" });
                    return NotFound(response);
                }
                response.Data = categories;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened" + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
