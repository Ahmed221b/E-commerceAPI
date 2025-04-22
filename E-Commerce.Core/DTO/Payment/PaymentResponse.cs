namespace E_Commerce.Core.DTO.Payment
{
    public class PaymentResponse
    {
        public string PaymentIntentId { get; set; }
        public string PaymentStatus { get; set; }
        public string ClientSecret { get; set; }
    }
}
