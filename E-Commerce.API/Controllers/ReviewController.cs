using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using E_Commerce.Core.DTO.CustomerReviews;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token");
        }


        [HttpPost]
        public async Task<ActionResult<CommonResponse<GetReviewDTO>>> AddReview(AddReviewDTO addReviewDTO)
        {
            var response = new CommonResponse<GetReviewDTO>();
            var userId = GetUserId();
            addReviewDTO.CustomerId = userId;
            var result = await _reviewService.AddReviewAsync(addReviewDTO);
            if (result.StatusCode == 200)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error
                {
                    Code = result.StatusCode,
                    Message = result.Message
                });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<CommonResponse<bool>>> DeleteReview(int productId)
        {
            var response = new CommonResponse<bool>();
            var userId = GetUserId();
            var result = await _reviewService.DeleteReview(new DeleteReviewDTO { CustomerId = userId, ProductId = productId });
            if (result.StatusCode == 200)
            {
                response.Data = result.Data;
                return NoContent();
            }
            else
            {
                response.Errors.Add(new Error
                {
                    Code = result.StatusCode,
                    Message = result.Message
                });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<CommonResponse<List<GetReviewDTO>>>> GetProductReviews(int productId)
        {
            var response = new CommonResponse<List<GetReviewDTO>>();
            var result = await _reviewService.GetProductReviews(productId);
            if (result.StatusCode == 200)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error
                {
                    Code = result.StatusCode,
                    Message = result.Message
                });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpPut]
        public async Task<ActionResult<CommonResponse<GetReviewDTO>>> UpdateReview(UpdateReviewDTO updateReviewDTO)
        {
            var response = new CommonResponse<GetReviewDTO>();
            var userId = GetUserId();
            updateReviewDTO.CustomerId = userId;
            var result = await _reviewService.UpdateReviewAsync(updateReviewDTO);
            if (result.StatusCode == 200)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error
                {
                    Code = result.StatusCode,
                    Message = result.Message
                });
                return StatusCode(result.StatusCode, response);
            }
        }
    }
}
