using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Product;

namespace E_Commerce.Core.DTO.Wishlist
{
    public class GetWishlistDTO
    {
        public int WishlistId { get; set; }
        public List<WishlistItemDTO> WishlistItems { get; set; } = new List<WishlistItemDTO>();
    }

    public class WishlistItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImageBase64 { get; set; }
    }
}
