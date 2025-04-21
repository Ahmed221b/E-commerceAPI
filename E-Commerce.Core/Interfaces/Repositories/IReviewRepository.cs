using E_Commerce.Models;

namespace E_Commerce.Core.Interfaces.Repositories
{
    public interface IReviewRepository : IBaseRepository<CustomerReview>
    {
        Task<CustomerReview> FindByProductIdAndCustomerId(int productId, string customerId);
        Task<List<CustomerReview>> GetProductReviews(int productId);

    }
}
