using System.Net;
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
        public async Task<ActionResult<CommonResponse<ColorDTO>>> CreateColor(AddColorDTO color)
        {
            var response = new CommonResponse<ColorDTO>();
            var result = await _colorService.CreateColor(color);
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
        [Route(nameof(GetColor))]
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

        [HttpDelete]
        [Route(nameof(DeleteColor))]
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

        [HttpGet]
        [Route(nameof(GetColors))]
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

        [HttpGet]
        [Route(nameof(SearchColors))]
        public async Task<ActionResult<CommonResponse<IEnumerable<ColorDTO>>>> SearchColors(string name)
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

        [HttpPut]
        [Route(nameof(UpdateColor))]
        public async Task<ActionResult<CommonResponse<ColorDTO>>> UpdateColor(UpdateColorDTO color)
        {
            var response = new CommonResponse<ColorDTO>();
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

