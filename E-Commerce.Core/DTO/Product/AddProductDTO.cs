using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Product
{
    public class AddProductDTO
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string ImageBase64 { get; set; }
        public int CategoryId { get; set; }
        public List<int> ColorIds { get; set; }


    }
}
