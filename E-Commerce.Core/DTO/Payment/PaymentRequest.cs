using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Payment
{
    public class PaymentRequest
    {
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string PaymentToken { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; } = "USD";
    }
}
