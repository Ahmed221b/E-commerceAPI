namespace E_Commerce.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();




    }
}
