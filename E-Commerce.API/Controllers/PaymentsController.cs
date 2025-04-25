using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using E_Commerce.Core.DTO.Payment;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentGatewayService _paymentService;
        public PaymentsController(IPaymentGatewayService paymentService)
        {
            _paymentService = paymentService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token");
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CommonResponse<PaymentResponse>>> ProcessPayment(PaymentRequest paymentRequest)
        {
            var response = new CommonResponse<PaymentResponse>();
            paymentRequest.CustomerId = GetUserId();
            var result = await _paymentService.ProcessPaymentAsync(paymentRequest);
            if (result.StatusCode == StatusCodes.Status200OK)
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
