using E_Commerce.Core.DTO.Cart;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface ICartService
    {
        Task<ServiceResult<CartItemsDTO>> AddItemToCart(AddToCartDTO addToCartDTO);
        Task<ServiceResult<CartItemsDTO>> GetCartItems(string userId);
    }
}
