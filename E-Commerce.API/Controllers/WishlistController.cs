using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using E_Commerce.Core.DTO.Wishlist;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpPost("items")]
        public async Task<ActionResult<CommonResponse<GetWishlistDTO>>> AddToWishList([FromBody] int productId)
        {
            var userId = GetUserId();
            var response = new CommonResponse<GetWishlistDTO>();
            var result = await _wishlistService.AddToWishlist(productId, userId);
            if (result.StatusCode == StatusCodes.Status200OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            response.Errors.Add(new Error
            {
                Code = result.StatusCode,
                Message = result.Message
            });
            return StatusCode(result.StatusCode, response);

        }

        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<CommonResponse<GetWishlistDTO>>> RemoveFromWishlist(int productId)
        {
            var userId = GetUserId();
            var response = new CommonResponse<GetWishlistDTO>();
            var result = await _wishlistService.RemoveFromWishlist(productId, userId);
            if (result.StatusCode == StatusCodes.Status200OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            response.Errors.Add(new Error
            {
                Code = result.StatusCode,
                Message = result.Message
            });
            return StatusCode(result.StatusCode, response);
        }

        [HttpGet("items")]
        public async Task<ActionResult<CommonResponse<GetWishlistDTO>>> GetWishlist()
        {
            var userId = GetUserId();
            var response = new CommonResponse<GetWishlistDTO>();
            var result = await _wishlistService.GetWishlist(userId);
            if (result.StatusCode == StatusCodes.Status200OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            response.Errors.Add(new Error
            {
                Code = result.StatusCode,
                Message = result.Message
            });
            return StatusCode(result.StatusCode, response);
        }
        [HttpDelete("items")]
        public async Task<ActionResult<CommonResponse<bool>>> ClearWishlist()
        {
            var userId = GetUserId();
            var response = new CommonResponse<bool>();
            var result = await _wishlistService.ClearWishlist(userId);
            if (result.StatusCode == StatusCodes.Status200OK)
            {
                response.Data = result.Data;
                return NoContent();
            }
            response.Errors.Add(new Error
            {
                Code = result.StatusCode,
                Message = result.Message
            });
            return StatusCode(result.StatusCode, response);
        }
    }
}
