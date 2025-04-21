using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.CustomerReviews
{
    public class GetReviewDTO
    {
        public int ProductId { get; set; }
        public string CustomerId { get; set; }
        public float Rate { get; set; }
        public string ReviewText { get; set; }
    }
}
