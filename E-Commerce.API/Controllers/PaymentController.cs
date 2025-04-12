using System.Net;
using E_Commerce.Core.DTO.Payment;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process-payment")]
        public async Task<ActionResult<CommonResponse<PaymentResult>>> ProcessPayment(PaymentRequest paymentRequest)
        {
            var response = new CommonResponse<PaymentResult>();
            var result = await _paymentService.ProcessPaymentAsync(paymentRequest);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error
                {
                    Code = result.StatusCode,
                    Message = result.Message
                });
                return StatusCode(result.StatusCode, response);
            }
        }
    }
}
