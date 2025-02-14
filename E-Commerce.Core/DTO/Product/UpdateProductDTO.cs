using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Product
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        public string? NewName { get; set; }
        public double? NewPrice { get; set; }
        public string? NewDescription { get; set; }
        public double? NewDiscount { get; set; }
        public string? NewImageBase64 { get; set; }
        public int? NewQuantity { get; set; }
        public int? NewCategoryId { get; set; }
        public List<int>? NewColorIds { get; set; }
    }
}
