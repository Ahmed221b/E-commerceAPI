using System.Net;
using E_Commerce.Core.DTO.Category;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // POST: api/categories
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constants.Admin},{Constants.Supervisor}")]
        public async Task<ActionResult<CommonResponse<GetCategoryDTO>>> AddCategory(AddCategoryDTO dto)
        {
            var response = new CommonResponse<GetCategoryDTO>();

            var result = await _categoryService.AddCategory(dto);
            if (result.StatusCode == (int)HttpStatusCode.Created)
            {
                response.Data = result.Data;
                return StatusCode(StatusCodes.Status201Created, response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<CommonResponse<IEnumerable<GetCategoryListDTO>>>> GetCategories()
        {
            var response = new CommonResponse<IEnumerable<GetCategoryListDTO>>();
            var result = await _categoryService.GetCategories();

            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CommonResponse<GetCategoryDTO>>> GetCategory(int id)
        {
            var response = new CommonResponse<GetCategoryDTO>();
            var result = await _categoryService.GetCategory(id);

            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        // PUT: api/categories/{id}
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constants.Admin},{Constants.Supervisor}")]
        public async Task<ActionResult<CommonResponse<GetCategoryDTO>>> UpdateCategory(int id, UpdateCategoryDTO dto)
        {
            var response = new CommonResponse<GetCategoryDTO>();
            dto.Id = id; // Ensure the ID from the route is used in the update DTO

            var result = await _categoryService.UpdateCategory(dto);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constants.Admin},{Constants.Supervisor}")]
        public async Task<ActionResult<CommonResponse<string>>> DeleteCategory(int id)
        {
            var response = new CommonResponse<string>();
            var result = await _categoryService.DeleteCategory(id);

            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = "Category Deleted Successfully";
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        // GET: api/categories/search
        [HttpGet("search")]
        public async Task<ActionResult<CommonResponse<IEnumerable<GetCategoryListDTO>>>> SearchCategories([FromQuery] string name)
        {
            var response = new CommonResponse<IEnumerable<GetCategoryListDTO>>();
            var result = await _categoryService.SearchCategories(name);

            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }
    }
}