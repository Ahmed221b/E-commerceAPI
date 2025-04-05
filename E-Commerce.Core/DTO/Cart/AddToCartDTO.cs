using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Cart
{
    public class AddToCartDTO
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
    }
}
