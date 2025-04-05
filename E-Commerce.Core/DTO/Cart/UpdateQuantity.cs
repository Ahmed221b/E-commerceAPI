using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Cart
{
    public class UpdateQuantity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
