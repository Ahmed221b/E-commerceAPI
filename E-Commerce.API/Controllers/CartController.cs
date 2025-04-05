
using E_Commerce.Core.DTO.Cart;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        //userId shouldn't be sent it will come from the token just till we figrure out the problem of Authorize
        [HttpPost("items")]
        public async Task<ActionResult<Response<CartItemsDTO>>> AddToCart([FromBody] int productId, string userId)
        {
            var response = new Response<CartItemsDTO>();
            //var userId = User.FindFirst("uid")?.Value;
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


        //userId shouldn't be sent it will come from the token just till we figrure out the problem of Authorize
        [HttpGet("items")]
        public async Task<ActionResult<Response<CartItemsDTO>>> GetCartItems(string userId)
        {
            var response = new Response<CartItemsDTO>();
            //var userId = User.FindFirst("uid")?.Value;
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

        //userId shouldn't be sent it will come from the token just till we figrure out the problem of Authorize
        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<Response<CartItemsDTO>>> RemoveItemFromCart(int productId, string userId)
        {
            var response = new Response<CartItemsDTO>();
            //var userId = User.FindFirst("uid")?.Value;
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

        //userId shouldn't be sent it will come from the token just till we figrure out the problem of Authorize
        [HttpPatch("items/{productId}")]
        public async Task<ActionResult<Response<CartItemsDTO>>> UpdateItemQuantity(int productId, int newQuantity, string userId)
        {
            var response = new Response<CartItemsDTO>();
            //var userId = User.FindFirst("uid")?.Value;
            var result = await _cartService.UpdateItemQuantity(userId, productId, newQuantity);
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

        //userId shouldn't be sent it will come from the token just till we figrure out the problem of Authorize
        [HttpDelete("items")]
        public async Task<ActionResult<Response<bool>>> ClearCart(string userId)
        {
            var response = new Response<bool>();
            //var userId = User.FindFirst("uid")?.Value;
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