using E_Commerce.Core.Custom_Exceptions;
using E_Commerce.Core.DTO.Color;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;
        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpPost]
        [Route(nameof(CreateColor))]
        public async Task<ActionResult<Response<GetColorDTO>>> CreateColor(ColorDTO color)
        {
            var response = new Response<GetColorDTO>();
            try
            {
                var newColor = await _colorService.CreateColor(color);
                response.Data = newColor;
                return Ok(response);
            }
            catch (ConflictException ex)
            {
                response.Errors.Add(new Error { Code = 409,Message = ex.Message});
                return Conflict(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 409, Message = "Unexpected error happened " + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError,response);
            }
        }

        [HttpGet]
        [Route(nameof(GetColor))]
        public async Task<ActionResult<Response<GetColorDTO>>> GetColor(int id)
        {
            var response = new Response<GetColorDTO>();
            try
            {
                var color = await _colorService.GetColor(id);
                if (color == null)
                {
                    response.Errors.Add(new Error { Code = 404, Message = "Color not found" });
                    return NotFound(response);
                }
                response.Data = color;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened " + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError,response);
            }
        }

        [HttpPost]
        [Route(nameof(DeleteColor))]
        public async Task<ActionResult<Response<string>>> DeleteColor(int id)
        {
            var response = new Response<string>();
            try
            {
                var result = await _colorService.DeleteColor(id);
                if (!result)
                {
                    response.Errors.Add(new Error { Code = 404, Message = $"No Color with Id {id} found" });
                    return NotFound(response);
                }
                response.Data = "Color deleted successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened " + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route(nameof(GetColors))]
        public async Task<ActionResult<Response<IEnumerable<GetColorDTO>>>> GetColors()
        {
            var response = new Response<IEnumerable<GetColorDTO>>();
            try
            {
                var colors = await _colorService.GetColors();
                if (colors.Count() == 0 )
                {
                    response.Errors.Add(new Error { Code = 404, Message = "No colors found" });
                    return NotFound(response);
                }
                response.Data = colors;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened " + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route(nameof(SearchColors))]
        public async Task<ActionResult<Response<IEnumerable<GetColorDTO>>>> SearchColors(string name)
        {
            var response = new Response<IEnumerable<GetColorDTO>>();
            try
            {
                var colors = await _colorService.SearchColors(name);
                if (colors.Count() == 0)
                {
                    response.Errors.Add(new Error { Code = 404, Message = "No match found" });
                    return NotFound(response);
                }
                response.Data = colors;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened " + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut]
        [Route(nameof(UpdateColor))]
        public async Task<ActionResult<Response<GetColorDTO>>> UpdateColor(UpdateColorDTO color)
        {
            var response = new Response<GetColorDTO>();
            try
            {
                var updatedColor = await _colorService.UpdateColor(color);
                if (updatedColor == null)
                {
                    response.Errors.Add(new Error { Code = 404, Message = "Color not found" });
                    return NotFound(response);
                }
                response.Data = updatedColor;
                return Ok(response);
            }
            catch (ConflictException ex)
            {
                response.Errors.Add(new Error { Code = 409, Message = ex.Message });
                return Conflict(response);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Unexpected error happened " + ex.Message });
                return StatusCode(StatusCodes.Status500InternalServerError,response);
            }
        }
    }
}
