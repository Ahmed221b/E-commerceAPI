using System.Net;
using E_Commerce.Core.DTO.Color;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/colors")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constants.Admin},{Constants.Supervisor}")]
    public class ColorsController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorsController(IColorService colorService)
        {
            _colorService = colorService;
        }

        // POST: api/colors
        [HttpPost]
        public async Task<ActionResult<CommonResponse<ColorDTO>>> CreateColor(AddColorDTO color)
        {
            var response = new CommonResponse<ColorDTO>();
            var result = await _colorService.CreateColor(color);

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

        // GET: api/colors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CommonResponse<ColorDTO>>> GetColor(int id)
        {
            var response = new CommonResponse<ColorDTO>();
            var result = await _colorService.GetColor(id);

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

        // DELETE: api/colors/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommonResponse<string>>> DeleteColor(int id)
        {
            var response = new CommonResponse<string>();
            var result = await _colorService.DeleteColor(id);

            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = "Color Deleted Successfully";
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        // GET: api/colors
        [HttpGet]
        public async Task<ActionResult<CommonResponse<IEnumerable<ColorDTO>>>> GetColors()
        {
            var response = new CommonResponse<IEnumerable<ColorDTO>>();
            var result = await _colorService.GetColors();

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

        // GET: api/colors/search
        [HttpGet("search")]
        public async Task<ActionResult<CommonResponse<IEnumerable<ColorDTO>>>> SearchColors([FromQuery] string name)
        {
            var response = new CommonResponse<IEnumerable<ColorDTO>>();
            var result = await _colorService.SearchColors(name);

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

        // PUT: api/colors/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<CommonResponse<ColorDTO>>> UpdateColor(int id, UpdateColorDTO color)
        {
            var response = new CommonResponse<ColorDTO>();
            color.Id = id; // Ensure the ID from the route is used for the update operation

            var result = await _colorService.UpdateColor(color);
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