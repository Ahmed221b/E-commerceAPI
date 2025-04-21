using E_Commerce.Core.DTO.CustomerReviews;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IReviewService
    {
        Task<ServiceResult<GetReviewDTO>> AddReviewAsync(AddReviewDTO addReviewDTO);
        Task<ServiceResult<bool>> DeleteReview(DeleteReviewDTO deleteReviewDTO);
        Task<ServiceResult<List<GetReviewDTO>>> GetProductReviews(int productId);
        Task<ServiceResult<GetReviewDTO>> UpdateReviewAsync(UpdateReviewDTO updateReviewDTO);




        //Task<bool> UpdateReviewAsync(int reviewId, string reviewText, int rating);
        //Task<bool> DeleteReviewAsync(int reviewId);
        //Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
        //Task<Review> GetReviewByIdAsync(int reviewId);
    }
}
