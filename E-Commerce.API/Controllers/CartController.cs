using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using E_Commerce.Core.DTO.Cart;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token");
        }




        [HttpPost("items")]
        public async Task<ActionResult<Response<CartItemsDTO>>> AddToCart([FromBody] int productId)
        {
            var response = new Response<CartItemsDTO>();
            var userId = GetUserId(); // Get user ID from token
            var result = await _cartService.AddItemToCart(new AddToCartDTO { ProductId = productId, UserId = userId });

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
        public async Task<ActionResult<Response<CartItemsDTO>>> GetCartItems()
        {
            var response = new Response<CartItemsDTO>();
            var userId = GetUserId(); // Get user ID from token
            var result = await _cartService.GetCartItems(userId);
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
        public async Task<ActionResult<Response<CartItemsDTO>>> RemoveItemFromCart(int productId)
        {
            var response = new Response<CartItemsDTO>();
            var userId = GetUserId(); // Get user ID from token
            var result = await _cartService.RemoveItemFromCart(userId, productId);
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

        [HttpPatch("items/{productId}")]
        public async Task<ActionResult<Response<CartItemsDTO>>> UpdateItemQuantity(UpdateQuantity updateQuantityDTO)
        {
            var response = new Response<CartItemsDTO>();
            var userId = GetUserId(); // Get user ID from token
            var result = await _cartService.UpdateItemQuantity(userId, updateQuantityDTO.ProductId, updateQuantityDTO.Quantity);
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
        public async Task<ActionResult<Response<bool>>> ClearCart()
        {
            var response = new Response<bool>();
            var userId = GetUserId(); // Get user ID from token
            var result = await _cartService.ClearCart(userId);
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