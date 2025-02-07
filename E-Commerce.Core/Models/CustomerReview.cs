namespace E_Commerce.Models
{
    public class CustomerReview
    {
        public float Rate { get; set; }
        public string ReviewText { get; set; }


        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
