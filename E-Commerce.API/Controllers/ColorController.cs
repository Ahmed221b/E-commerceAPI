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
        public async Task<ActionResult<Response<GetColorDTO>>> CreateColor(AddColorDTO color)
        {
            var response = new Response<GetColorDTO>();
            var result = await _colorService.CreateColor(color);
            if (result.StatusCode == 409)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return Conflict(response);
            }
            else if (result.StatusCode == 500)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            response.Data = result.Data;
            return Ok(response);
        }

        [HttpGet]
        [Route(nameof(GetColor))]
        public async Task<ActionResult<Response<GetColorDTO>>> GetColor(int id)
        {
            var response = new Response<GetColorDTO>();
            var result = await _colorService.GetColor(id);
            if (result.StatusCode == 404)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return NotFound(response);
            }
            else if (result.StatusCode == 500)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            response.Data = result.Data;
            return Ok(response);
        }

        [HttpPost]
        [Route(nameof(DeleteColor))]
        public async Task<ActionResult<Response<string>>> DeleteColor(int id)
        {
            var response = new Response<string>();
            var result = await _colorService.DeleteColor(id);
            if (result.StatusCode == 404)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return NotFound(response);
            }
            else if (result.StatusCode == 500)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            response.Data = "Color deleted successfully";
            return Ok(response);
        }

        [HttpGet]
        [Route(nameof(GetColors))]
        public async Task<ActionResult<Response<IEnumerable<GetColorDTO>>>> GetColors()
        {
            var response = new Response<IEnumerable<GetColorDTO>>();
            var result = await _colorService.GetColors();
            if (result.StatusCode == 404)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return NotFound(response);
            }
            else if (result.StatusCode == 500)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
    
            response.Data = result.Data;
            return Ok(response);

        }

        [HttpGet]
        [Route(nameof(SearchColors))]
        public async Task<ActionResult<Response<IEnumerable<GetColorDTO>>>> SearchColors(string name)
        {
            var response = new Response<IEnumerable<GetColorDTO>>();
            var result = await _colorService.SearchColors(name);
            if (result.StatusCode == 404)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return NotFound(response);
            }
            else if (result.StatusCode == 500)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            response.Data = result.Data;
            return Ok(response);
        }

        [HttpPut]
        [Route(nameof(UpdateColor))]
        public async Task<ActionResult<Response<GetColorDTO>>> UpdateColor(UpdateColorDTO color)
        {
            var response = new Response<GetColorDTO>();
            var result = await _colorService.UpdateColor(color);
            if (result.StatusCode == 404)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return NotFound(response);
            }
            else if (result.StatusCode == 409)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return Conflict(response);
            }
            else if (result.StatusCode == 500)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            response.Data = result.Data;
            return Ok(response);
        }
    }
}

