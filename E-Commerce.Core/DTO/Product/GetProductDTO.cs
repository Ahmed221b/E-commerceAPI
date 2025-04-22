using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Color;
using E_Commerce.Core.DTO.CustomerReviews;

namespace E_Commerce.Core.DTO.Product
{
    public class GetProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public double Discount { get; set; }
        public double Rate { get; set; }
        public int Quantity { get; set; }
        public string ImageBase64 { get; set; }
        public string Category { get; set; }
        public List<string> Colors { get; set; }
        public List<GetReviewDTO> Reviews { get; set; }


    }
}
