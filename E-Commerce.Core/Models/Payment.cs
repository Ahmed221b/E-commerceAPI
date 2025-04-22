using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Models;
using static E_Commerce.Core.Shared.Constants;

namespace E_Commerce.Core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentIntentId { get; set; }
        public int? OrderId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? FailureReason { get; set; }
        public string PaymentStatus { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Order Order { get; set; }
    }
}
