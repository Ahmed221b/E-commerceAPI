
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
        [HttpPost("AddToCart")]
        public async Task<ActionResult<Response<CartItemsDTO>>> AddToCart(int productId,string userId)
        {
            var response = new Response<CartItemsDTO>();
            //var userId = User.FindFirst("uid")?.Value;
            var result = await _cartService.AddItemToCart(new AddToCartDTO { ProductId = productId,UserId = userId});

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
        [HttpGet("GetCartItems")]
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
    }
}