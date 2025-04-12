using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Payment
{
    public class PaymentResult
    {
        public bool IsSuccess { get; set; }
        public int PaymentRecordId { get; set; }
        public string PaymentIntentId { get; set; }
        public string Message { get; set; }
        public string PaymentStatus { get; set; }
    }
}
