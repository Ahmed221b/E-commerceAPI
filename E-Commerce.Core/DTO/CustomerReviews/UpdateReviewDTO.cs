using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.CustomerReviews
{
    public class UpdateReviewDTO
    {
        public float NewRate { get; set; }
        public string NewReviewText { get; set; }
        public int ProductId { get; set; }
        public string? CustomerId { get; set; }
    }
}
