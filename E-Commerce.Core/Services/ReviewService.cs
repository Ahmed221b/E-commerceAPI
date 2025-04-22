using System.Net;
using AutoMapper;
using E_Commerce.Core.DTO.CustomerReviews;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;

namespace E_Commerce.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReviewService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResult<GetReviewDTO>> AddReviewAsync(AddReviewDTO addReviewDTO)
        {
            try
            {
                
                var porduct = await _unitOfWork.ProductRepository.GetById(addReviewDTO.ProductId);
                if (porduct == null)
                {
                    return new ServiceResult<GetReviewDTO>("Product not found", (int)HttpStatusCode.NotFound);
                }
                var review = await _unitOfWork.ReviewRepository.FindByProductIdAndCustomerId( addReviewDTO.ProductId, addReviewDTO.CustomerId);
                if (review != null)
                {
                    return new ServiceResult<GetReviewDTO>("You have already reviewed this product", (int)HttpStatusCode.Conflict);
                }
                var newReview = new CustomerReview
                {
                    Rate = addReviewDTO.Rate,
                    ReviewText = addReviewDTO.ReviewText,
                    ProductId = addReviewDTO.ProductId,
                    CustomerId = addReviewDTO.CustomerId
                };
                var addedReview = await _unitOfWork.ReviewRepository.AddAsync(newReview);
                await _unitOfWork.Complete();
                var result = _mapper.Map<GetReviewDTO>(addedReview);
                return new ServiceResult<GetReviewDTO>(result);
            }
            catch (Exception ex)
            {
                return new ServiceResult<GetReviewDTO>("Unexpected error: "+ ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<bool>> DeleteReview(DeleteReviewDTO deleteReviewDTO)
        {
            try
            {
                var review = await _unitOfWork.ReviewRepository.FindByProductIdAndCustomerId(deleteReviewDTO.ProductId, deleteReviewDTO.CustomerId);
                if (review == null)
                {
                    return new ServiceResult<bool>("Review not found", (int)HttpStatusCode.NotFound);
                }
                _unitOfWork.ReviewRepository.Remove(review);
                 await _unitOfWork.Complete();
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>("Unexpected error: " + ex.Message, (int)HttpStatusCode.InternalServerError);

            }
        }

        public async Task<ServiceResult<List<GetReviewDTO>>> GetProductReviews(int productId)
        {
            try
            {
                var reviews = await _unitOfWork.ReviewRepository.GetProductReviews(productId);
                if (reviews == null || reviews.Count == 0)
                {
                    return new ServiceResult<List<GetReviewDTO>>("No reviews found", (int)HttpStatusCode.NotFound);
                }
                var result = _mapper.Map<List<GetReviewDTO>>(reviews);
                return new ServiceResult<List<GetReviewDTO>>(result);
            }
            catch (Exception ex)
            {
                return new ServiceResult<List<GetReviewDTO>>("Unexpected error: " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }



        }

        public async Task<ServiceResult<GetReviewDTO>> UpdateReviewAsync(UpdateReviewDTO updateReviewDTO)
        {
            try
            {
                var review = await _unitOfWork.ReviewRepository.FindByProductIdAndCustomerId(updateReviewDTO.ProductId, updateReviewDTO.CustomerId);
                if (review == null)
                {
                    return new ServiceResult<GetReviewDTO>("Review not found", (int)HttpStatusCode.NotFound);
                }
                review.Rate = updateReviewDTO.NewRate;
                review.ReviewText = updateReviewDTO.NewReviewText;
                _unitOfWork.ReviewRepository.Update(review);
                await _unitOfWork.Complete();
                var result = _mapper.Map<GetReviewDTO>(review);
                return new ServiceResult<GetReviewDTO>(result);

            }
            catch (Exception ex)
            {
                return new ServiceResult<GetReviewDTO>("Unexpected error: " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
