using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.EF.Repositories
{
    public class ReviewRepository : BaseRepository<CustomerReview>, IReviewRepository
    {
        private readonly ApplicationDBContext _context;
        public ReviewRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CustomerReview> FindByProductIdAndCustomerId(int productId, string customerId)
        {
            return await _context.CustomerReviews
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.CustomerId == customerId);
        }

        public async Task<List<CustomerReview>> GetProductReviews(int productId)
        {
            return await _context.CustomerReviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }
    }
    
    
}
