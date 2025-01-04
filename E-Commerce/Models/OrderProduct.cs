namespace E_Commerce.Models
{
    public class OrderProduct
    {

        public int Quantity { get; set; }
        public double Price { get; set; }
            

        public int ProductId { get; set; }
        public Product Product { get; set; }


        public int OrderId { get; set; }
        public Order Order { get; set; }
    }

}
