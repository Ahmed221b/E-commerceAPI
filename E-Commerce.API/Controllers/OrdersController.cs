using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using E_Commerce.Core.DTO.Order;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constants.Admin},{Constants.Supervisor}")]
        public async Task<ActionResult<CommonResponse<List<GetOrderDTO>>>> GetAllOrders()
        {
            var response = new CommonResponse<List<GetOrderDTO>>(); 
            var result = await _orderService.GetAllOrdersAsync();
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
            return StatusCode(result.StatusCode,response);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<CommonResponse<GetOrderDTO>>> GetOrderById(int orderId)
        {
            var response = new CommonResponse<GetOrderDTO>();
            var result = await _orderService.GetOrderByIdAsync(orderId);
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



        [HttpGet]
        [Route("myorders")]
        public async Task<ActionResult<CommonResponse<List<GetOrderDTO>>>> GetOrdersByCustomerId()
        {
            string customerId = GetUserId();
            var response = new CommonResponse<List<GetOrderDTO>>();
            var result = await _orderService.GetOrdersByCustomerIdAsync(customerId);
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


        [HttpPost]
        [Route("cancel-order/{orderId}")]
        public async Task<ActionResult<CommonResponse<GetOrderDTO>>> CancelOrder(int orderId)
        {
            var response = new CommonResponse<GetOrderDTO>();
            var result = await _orderService.CancelOrder(orderId);
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


        [HttpPut]
        [Route("update-order-status/{orderId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constants.Admin},{Constants.Supervisor}")]
        public async Task<ActionResult<CommonResponse<GetOrderDTO>>> UpdateOrderStatus(int orderId, [FromBody] string status)
        {
            var response = new CommonResponse<GetOrderDTO>();
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);
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

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token");
        }

    }
}
