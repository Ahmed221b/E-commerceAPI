namespace E_Commerce.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();




    }
}
