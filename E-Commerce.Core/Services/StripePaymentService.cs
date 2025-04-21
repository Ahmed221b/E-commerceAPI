using System.Net;
using E_Commerce.Core.Configuration;
using E_Commerce.Core.DTO.Payment;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.Core.Services
{
    public class StripePaymentService : IPaymentService
    {
        private readonly StripeSettings _stripeSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public StripePaymentService(IOptions<StripeSettings> options, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _stripeSettings = options.Value;
            Stripe.StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<ServiceResult<PaymentResponse>> ProcessPaymentAsync(PaymentRequest paymentRequset)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(paymentRequset.CustomerId);
                if (user == null)
                {
                    return new ServiceResult<PaymentResponse>("User not found", (int)HttpStatusCode.NotFound);
                }
                var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(paymentRequset.CustomerId);
                if (!cart.CartItems.Any())
                {
                    return new ServiceResult<PaymentResponse>("No items in the cart", (int)HttpStatusCode.BadRequest);
                }
                var paymentIntentOptions = new PaymentIntentCreateOptions
                {
                    Amount = (long)cart.TotalPrice*100,
                    Currency = paymentRequset.Currency,
                    ReceiptEmail = paymentRequset.CustomerEmail,
                    Metadata = new Dictionary<string, string>
                    {
                        { "customerId", paymentRequset.CustomerId },
                        { "orderContext", "Temporary placeholder, replace on webhook" }
                    }
                };

                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.CreateAsync(paymentIntentOptions);

                var response = new PaymentResponse
                {
                    PaymentIntentId = paymentIntent.Id,
                    ClientSecret = paymentIntent.ClientSecret,
                    PaymentStatus = paymentIntent.Status
                };
                return new ServiceResult<PaymentResponse>(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult<PaymentResponse>("Error processing payment: " + ex.Message,(int)HttpStatusCode.InternalServerError);
            }
        }


      
    }
}
