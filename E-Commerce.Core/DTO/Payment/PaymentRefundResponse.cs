namespace E_Commerce.Core.DTO.Payment
{
    public class PaymentRefundResponse
    {
        public string PaymentIntentId { get; set; }
        public string RefundStatus { get; set; }
    }
}
