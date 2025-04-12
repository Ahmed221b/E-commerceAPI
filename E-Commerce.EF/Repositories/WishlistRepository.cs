using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.EF.Repositories
{
    public class WishlistRepository : BaseRepository<Wishlist>, IWishlistRepository
    {
        private readonly ApplicationDBContext _context;
        public WishlistRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Wishlist> GetWishlistByCustomerId(string customerId)
        {
            return await _context.Wishlists.FirstOrDefaultAsync(w => w.CustomerId == customerId);
        }
    }
}
