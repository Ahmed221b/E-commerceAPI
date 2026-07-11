namespace E_Commerce.Core.DTO.Cart
{
    public class AddToCartDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? UserId { get; set; }
    }
}
