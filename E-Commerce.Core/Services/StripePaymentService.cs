using System.Threading.Tasks;
using E_Commerce.Core.Configuration;
using E_Commerce.Core.DTO.Payment;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Models;
using E_Commerce.Core.Shared;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.Core.Services
{
    internal class StripePaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private StripeSettings _stripeSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        public StripePaymentService(IUnitOfWork unitOfWork, IOptions<StripeSettings> options, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _stripeSettings = options.Value;
            _userManager = userManager;
        }
        public async Task<ServiceResult<PaymentResult>> ProcessPaymentAsync(PaymentRequest paymentRequest)
        {
            try
            {
                var newPayment = await _unitOfWork.PaymentRepository.AddAsync(
                    new Payment
                    {
                        Amount = paymentRequest.Amount,
                        Currency = paymentRequest.Currency,
                        CustomerId = paymentRequest.CustomerId,
                        CustomerEmail = paymentRequest.CustomerEmail,
                        CreatedAt = DateTime.UtcNow,
                        PaymentStatus = Constants.PaymentStatus.Pending.ToString(),

                    }
                );
                var paymentIntentService = new PaymentIntentService();
                var paymentMethodService = new PaymentMethodService();
                var stripeCustomerID = await GetStripeCustomerId(paymentRequest.CustomerId); // Get or create Stripe Customer ID
                // Attach the payment method token to the customer (if applicable)
                var paymentMethod = await paymentMethodService.AttachAsync(
                    paymentRequest.PaymentToken,  // The payment token received from frontend (e.g., "tok_1...")
                    new PaymentMethodAttachOptions
                    {
                        Customer = stripeCustomerID // Pass the customer ID if you have one
                    }
                );
                var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
                {
                    Amount = (long)(paymentRequest.Amount * 100), // Convert to cents
                    Currency = paymentRequest.Currency.ToLower(),
                    PaymentMethod = paymentMethod.Id,  // Use the PaymentMethod ID here
                    Confirm = true,
                    Description = $"Payment for {paymentRequest.CustomerEmail}"
                });


                if (paymentIntent.Status == "succeeded")
                {
                    newPayment.IsSuccess = true;
                    newPayment.PaymentStatus = Constants.PaymentStatus.Completed.ToString();
                    newPayment.PaymentIntentId = paymentIntent.Id;
                    return new ServiceResult<PaymentResult>(new PaymentResult
                    {
                        IsSuccess = true,
                        PaymentStatus = Constants.PaymentStatus.Completed.ToString(),
                        PaymentRecordId = newPayment.Id,
                        Message = "Payment succeeded",
                        PaymentIntentId = paymentIntent.Id,
                    });
                    //Save order to the database
                }
                else
                {
                    newPayment.IsSuccess = false;
                    newPayment.PaymentStatus = Constants.PaymentStatus.Failed.ToString();
                    newPayment.FailureReason = paymentIntent.Status;
                    newPayment.PaymentIntentId = paymentIntent.Id;
                    return new ServiceResult<PaymentResult>(new PaymentResult
                    {
                        IsSuccess = false,
                        PaymentStatus = Constants.PaymentStatus.Failed.ToString(),
                        Message = paymentIntent.Status,
                        PaymentIntentId = paymentIntent.Id,
                    });
                }

            }
            catch (Exception ex)
            {
                return new ServiceResult<PaymentResult>(new PaymentResult
                {
                    IsSuccess = false,
                    PaymentStatus = Constants.PaymentStatus.Failed.ToString(),
                    Message = "Payment failed: " + ex.Message,

                });

            }
            finally
            {
                await _unitOfWork.Complete();
            }
        }


        private async Task<string> GetStripeCustomerId(string customerId)
        {
            var user = await _userManager.FindByIdAsync(customerId) as E_Commerce.Models.Customer; // Fetch user by their ID

            if (string.IsNullOrEmpty(user.StripeCustomerId))
            {
                // User doesn't have a Stripe Customer ID, create one
                var customerService = new CustomerService();
                var customer = await customerService.CreateAsync(new CustomerCreateOptions
                {
                    Email = user.Email,  // Use the customer's email or other details
                    Name = user.UserName     // You can add more details if needed
                });

                // Save the Stripe Customer ID to the user's record
                user.StripeCustomerId = customer.Id;
                try
                {
                    await _userManager.UpdateAsync(user);

                }
                catch (Exception ex)
                {

                }
                
                return user.StripeCustomerId;
            }
            else
            {
                return user.StripeCustomerId;
            }

        }
    }
}
