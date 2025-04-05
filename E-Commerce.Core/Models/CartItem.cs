namespace E_Commerce.Models
{
    public class CartItem
    {
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public double ItemPrice { get; set; }

        public double TotalPriceOfItems => ItemPrice * Quantity;    

        public DateTime AddedOn { get; set; }

    }
}
