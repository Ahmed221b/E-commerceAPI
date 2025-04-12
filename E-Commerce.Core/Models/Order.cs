using E_Commerce.Core.Models;

namespace E_Commerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public String Status { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }




        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int PaymentId { get; set; }
        public virtual Payment Payment { get; set; }


        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    }
}
