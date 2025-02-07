namespace E_Commerce.Models
{
    public class Customer : ApplicationUser
    {
        public string Address { get; set; }
        public DateOnly DataOfBirth { get; set; }
        public int WishlidtId { get; set; }
        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Wishlist Wishlist { get; set; }
        public virtual ICollection<CustomerReview> CustomerReviews { get; set; } = new List<CustomerReview>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
