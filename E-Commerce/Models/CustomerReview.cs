namespace E_Commerce.Models
{
    public class CustomerReview
    {
        public float Rate { get; set; }
        public string ReviewText { get; set; }


        public int ProductID { get; set; }
        public Product Product { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
