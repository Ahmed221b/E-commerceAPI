namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        public double Discount { get; set; }
        public double Rate => !CustomerReviews.Any() ? 0 : CustomerReviews.Average(p => p.Rate);
        public int Quantity { get; set; }
        public byte[] Image { get; set; }


        public virtual ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public virtual ICollection<CustomerReview> CustomerReviews { get; set; } = new List<CustomerReview>();
        public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();



        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }


    }
}
