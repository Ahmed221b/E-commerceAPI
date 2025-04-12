
using E_Commerce.Core.DTO.Wishlist;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IWishlistService
    {
        Task<ServiceResult<GetWishlistDTO>> AddToWishlist(int productId, string customerId);
        Task<ServiceResult<GetWishlistDTO>> RemoveFromWishlist(int productId, string customerId);
        Task<ServiceResult<GetWishlistDTO>> GetWishlist(string customerId);
        Task<ServiceResult<bool>> ClearWishlist(string customerId);
    }
}
