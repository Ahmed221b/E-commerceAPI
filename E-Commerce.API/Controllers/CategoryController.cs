using System.Net;
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
        public async Task<ActionResult<CommonResponse<GetCategoryDTO>>> AddCategory(AddCategoryDTO dto)
        {
            var response = new CommonResponse<GetCategoryDTO>();

            var result = await _categoryService.AddCategory(dto);
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


        [HttpGet]
        [Route(nameof(GetCategories))]
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

        [HttpGet]
        [Route(nameof(GetCategory))]
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

        [HttpPut]
        [Route(nameof(UpdateCategory))]
        public async Task<ActionResult<CommonResponse<GetCategoryDTO>>> UpdateCategory(UpdateCategoryDTO dto)
        {
            var response = new CommonResponse<GetCategoryDTO>();
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
        
        
        [HttpDelete]
        [Route(nameof(DeleteCategory))]
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

        [HttpPost]
        [Route(nameof(SearchCategories))]
        public async Task<ActionResult<CommonResponse<IEnumerable<GetCategoryListDTO>>>> SearchCategories(string name)
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
