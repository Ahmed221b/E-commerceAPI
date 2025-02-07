namespace E_Commerce.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }


        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual Customer Customer { get; set; }
        public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();

        
    }
}
