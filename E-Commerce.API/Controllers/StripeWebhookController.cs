using E_Commerce.Core.Configuration;
using E_Commerce.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly StripeSettings stripeSettings;
        private readonly ICartService _cartService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        public StripeWebhookController(IOptions<StripeSettings> options, ICartService cartService, IPaymentService paymentService, IOrderService orderService)
        {
            stripeSettings = options.Value;
            _cartService = cartService;
            _paymentService = paymentService;
            _orderService = orderService;
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                // Validate the Stripe signature
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    stripeSettings.WebhookKey
                );

                string customerId = string.Empty;
                // Handle Stripe event types
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        customerId = paymentIntent.Metadata["customerId"];
                        var result = await _orderService.CreateOrderAsync(customerId);
                        var newOrder = result.Data;
                        await _cartService.ClearCart(customerId);
                        await _paymentService.CreatePaymentRecordAsync(true, customerId, paymentIntent.ReceiptEmail, paymentIntent.Id, paymentIntent.Currency, paymentIntent.Amount,newOrder.OrderId);

                        /*TODO
                         * - Send a mail to the user with the order confirmation and the invoice.
                        */
                        break;

                    case "payment_intent.payment_failed":
                        var failedPayment = stripeEvent.Data.Object as PaymentIntent;
                        customerId = failedPayment.Metadata["customerId"];
                        await _paymentService.CreatePaymentRecordAsync
                            (false, customerId,failedPayment.Customer.Email, failedPayment.Id,failedPayment.Currency,failedPayment.Amount,null);
                        break;

                    default:
                        break;
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest($"Stripe error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
