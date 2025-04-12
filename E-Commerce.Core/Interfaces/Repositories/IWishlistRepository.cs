
using E_Commerce.Models;

namespace E_Commerce.Core.Interfaces.Repositories
{
    public interface IWishlistRepository : IBaseRepository<Wishlist>
    {
        Task<Wishlist> GetWishlistByCustomerId(string customerId);
    }
}
