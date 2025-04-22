using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<ServiceResult<bool>> CreatePaymentRecordAsync
            (bool IsSucceeded, string customerId, string customerEmail, string paymentIntentId, string currency, double amount,int? orderId);

    }
}
