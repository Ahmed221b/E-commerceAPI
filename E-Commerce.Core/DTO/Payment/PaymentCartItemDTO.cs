namespace E_Commerce.Core.DTO.Payment
{
    public class PaymentCartItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}
