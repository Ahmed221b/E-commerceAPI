namespace E_Commerce.Models
{
    public class Customer : ApplicationUser
    {
        public string Address { get; set; }
        public DateOnly DataOfBirth { get; set; }
        public int WishlidtId { get; set; }
        public int CartId { get; set; }

        public Cart Cart { get; set; }
        public Wishlist Wishlist { get; set; }
        public ICollection<CustomerReview> CustomerReviews { get; set; } = new List<CustomerReview>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
