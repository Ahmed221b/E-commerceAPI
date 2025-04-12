using AutoMapper;
using E_Commerce.Core.DTO.Wishlist;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WishlistService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResult<GetWishlistDTO>> AddToWishlist(int productId, string customerId)
        {
            try
            {
                var addedWishlist = new Wishlist();
                var product = await _unitOfWork.ProductRepository.GetById(productId);
                if (product == null)
                {
                    return new ServiceResult<GetWishlistDTO>("Product not found", (int)StatusCodes.Status404NotFound);
                }

                var wishlist = await _unitOfWork.WishlistRepository.GetWishlistByCustomerId(customerId);
                if (wishlist == null)
                {
                    
                    var newWishlist = new Wishlist
                    {
                        CustomerId = customerId,
                        WishlistItems = new List<WishlistItem>
                        {
                            new WishlistItem
                            {
                                ProductId = productId
                            }
                        }
                    };
                    addedWishlist = await _unitOfWork.WishlistRepository.AddAsync(newWishlist);
                }
                else
                {
                    if (wishlist.WishlistItems.Any(x => x.ProductId == productId))
                    {
                        return new ServiceResult<GetWishlistDTO>("Product already in wishlist", (int)StatusCodes.Status409Conflict);
                    }
                    wishlist.WishlistItems.Add(new WishlistItem
                    {
                        ProductId = productId
                    });
                    addedWishlist = _unitOfWork.WishlistRepository.Update(wishlist);
                }
                var result = _mapper.Map<GetWishlistDTO>(addedWishlist);
                await _unitOfWork.Complete();
                return new ServiceResult<GetWishlistDTO>(result);

            }
            catch (Exception ex)
            {
                return new ServiceResult<GetWishlistDTO>("Unexpected error occurred: " + ex.Message,(int)StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ServiceResult<bool>> ClearWishlist(string customerId)
        {
            try
            {
                var wishlist = await _unitOfWork.WishlistRepository.GetWishlistByCustomerId(customerId);
                if (wishlist == null)
                {
                    return new ServiceResult<bool>("Wishlist not found", (int)StatusCodes.Status404NotFound);
                }
                wishlist.WishlistItems.Clear();
                _unitOfWork.WishlistRepository.Update(wishlist);
                await _unitOfWork.Complete();
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>("Unexpected error occurred: " + ex.Message, (int)StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ServiceResult<GetWishlistDTO>> GetWishlist(string customerId)
        {
            try
            {
                var wishlist = await _unitOfWork.WishlistRepository.GetWishlistByCustomerId(customerId);
                if (wishlist == null)
                {
                    return new ServiceResult<GetWishlistDTO>("Wishlist not found", (int)StatusCodes.Status404NotFound);
                }
                var result = _mapper.Map<GetWishlistDTO>(wishlist);
                return new ServiceResult<GetWishlistDTO>(result);
            }
            catch (Exception ex)
            {
                return new ServiceResult<GetWishlistDTO>("Unexpected error occurred: " + ex.Message, (int)StatusCodes.Status500InternalServerError);
            }

        }

        public async Task<ServiceResult<GetWishlistDTO>> RemoveFromWishlist(int productId, string customerId)
        {
            try
            {
                var wishlist = await _unitOfWork.WishlistRepository.GetWishlistByCustomerId(customerId);
                if (wishlist == null)
                {
                    return new ServiceResult<GetWishlistDTO>("Wishlist not found", (int)StatusCodes.Status404NotFound);
                }
                var wishlistItem = wishlist.WishlistItems.FirstOrDefault(x => x.ProductId == productId);
                if (wishlistItem == null)
                {
                    return new ServiceResult<GetWishlistDTO>("Product not found in wishlist", (int)StatusCodes.Status404NotFound);
                }
                wishlist.WishlistItems.Remove(wishlistItem);
                _unitOfWork.WishlistRepository.Update(wishlist);
                await _unitOfWork.Complete();
                var result = _mapper.Map<GetWishlistDTO>(wishlist);
                return new ServiceResult<GetWishlistDTO>(result);
            }
            catch (Exception ex)
            {
                return new ServiceResult<GetWishlistDTO>("Unexpected error occurred: " + ex.Message, (int)StatusCodes.Status500InternalServerError);
            }

        }
    }
}
