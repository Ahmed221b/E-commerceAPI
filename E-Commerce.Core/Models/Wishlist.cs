namespace E_Commerce.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }


        public ICollection<Product> Products { get; set; } = new List<Product>();
        public Customer Customer { get; set; }

        public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();

        
    }
}
