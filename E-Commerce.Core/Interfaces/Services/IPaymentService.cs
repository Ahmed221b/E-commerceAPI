using E_Commerce.Core.DTO.Payment;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<ServiceResult<PaymentResponse>> ProcessPaymentAsync(PaymentRequest paymentRequset);
    }
}
