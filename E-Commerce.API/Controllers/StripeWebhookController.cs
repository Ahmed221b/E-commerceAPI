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
        private readonly ICartService cartService;
        public StripeWebhookController(IOptions<StripeSettings> options, ICartService cartService)
        {
            stripeSettings = options.Value;
            this.cartService = cartService;
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

                // Handle Stripe event types
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        await cartService.ClearCart(paymentIntent.Metadata["customerId"]);
                        /*TODO
                         * 1- Create new order to with the suitable status.
                         * 2- Send a mail to the user with the order confirmation and the invoice.
                         * 3- Create a success payment record in the payments table in my DB.
            
                        */
                        break;

                    case "payment_intent.payment_failed":
                        var failedPayment = stripeEvent.Data.Object as PaymentIntent;
                        /*TODO
                         * 1- Create a faild payment record in the payments table in my DB.
                         */
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
