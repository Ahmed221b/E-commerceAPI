namespace E_Commerce.Models
{
    public class WishlistItem
    {

        public int WishlistId { get; set; }
        public virtual Wishlist Wishlist { get; set; }


        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
