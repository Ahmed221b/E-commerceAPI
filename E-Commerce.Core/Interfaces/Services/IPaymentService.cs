using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Payment;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<ServiceResult<PaymentResult>> ProcessPaymentAsync(PaymentRequest paymentRequest);

    }
}
