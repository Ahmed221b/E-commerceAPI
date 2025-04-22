using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Models;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Http;
using static E_Commerce.Core.Shared.Constants;

namespace E_Commerce.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitofwork;
        public PaymentService(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task<ServiceResult<bool>> CreatePaymentRecordAsync(bool IsSucceeded, string customerId, string customerEmail, string paymentIntentId, string currency, double amount,int? orderId)
        {
            try
            {

                var newPaymentRecord = new Payment
                {
                    IsSuccess = IsSucceeded,
                    CreatedAt = DateTime.UtcNow,
                    PaymentStatus = IsSucceeded ? PaymentStatus.Completed.ToString() : PaymentStatus.Failed.ToString(),
                    FailureReason = IsSucceeded ? null : "Faild to create Payment Intent",
                    Amount = amount,
                    Currency = currency,
                    CustomerId = customerId,
                    CustomerEmail = customerEmail,
                    PaymentIntentId = paymentIntentId,
                    OrderId = orderId

                };
                await _unitofwork.PaymentRepository.AddAsync(newPaymentRecord);
                await _unitofwork.Complete();
                return new ServiceResult<bool>(true);

            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>("Unexpected Error while creating payment record: " + ex, (int)StatusCodes.Status500InternalServerError);
            }
        }
    }
}
