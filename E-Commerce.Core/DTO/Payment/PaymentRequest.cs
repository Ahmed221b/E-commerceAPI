using E_Commerce.Core.DTO.Cart;

namespace E_Commerce.Core.DTO.Payment
{
    public class PaymentRequest
    {
        public string? CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string Currency { get; set; }

    }
}
