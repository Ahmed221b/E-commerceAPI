namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public double Discount { get; set; }
        public double Rate { get; set; }
        public int Quantity { get; set; }
        public byte[] Image { get; set; }


        public ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        public ICollection<CustomerReview> CustomerReviews { get; set; } = new List<CustomerReview>();
        public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();



        public int CategoryId { get; set; }
        public Category Category { get; set; }


    }
}
