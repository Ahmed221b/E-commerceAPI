using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Core.DTO.Cart;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<CartItemsDTO>> AddItemToCart(AddToCartDTO addToCartDTO)
        {
            try
            {
                Cart addedCart = new Cart();
                var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(addToCartDTO.UserId);

                // 1. جبنا المنتج مرة واحدة فوق عشان نستخدمه براحتنا
                var product = await _unitOfWork.ProductRepository.GetById(addToCartDTO.ProductId);
                if (product == null)
                {
                    return new ServiceResult<CartItemsDTO>("Product not found", (int)StatusCodes.Status404NotFound);
                }

                // 2. تأكيد أولي إن الكمية المطلوبة موجودة في المخزون أساساً
                if (product.Quantity < addToCartDTO.Quantity)
                {
                    return new ServiceResult<CartItemsDTO>("Product is out of stock or requested quantity exceeds available stock", (int)StatusCodes.Status400BadRequest);
                }

                if (cart == null)
                {
                    var newCart = new Cart { CustomerId = addToCartDTO.UserId };
                    newCart.CartItems.Add(new CartItem
                    {
                        ProductId = product.Id,
                        Quantity = addToCartDTO.Quantity, // أخدنا الكمية من الـ DTO
                        Product = product,
                        ItemPrice = product.Price,
                        AddedOn = DateTime.UtcNow,
                    });
                    addedCart = await _unitOfWork.CartRepository.AddAsync(newCart);
                }
                else
                {
                    if (cart.CartItems.Any(x => x.ProductId == product.Id))
                    {
                        var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == product.Id);

                        // 3. تأكيد إن الكمية القديمة + الزيادة الجديدة مش هتعدي المخزون
                        if (cartItem.Quantity + addToCartDTO.Quantity > product.Quantity)
                        {
                            return new ServiceResult<CartItemsDTO>($"Cannot add more. You have {cartItem.Quantity} in cart and only {product.Quantity} in stock.", (int)StatusCodes.Status400BadRequest);
                        }

                        cartItem.Quantity += addToCartDTO.Quantity; // صلحنا الثابت وخليناه يزود الكمية المبعوتة
                    }
                    else
                    {
                        cart.CartItems.Add(new CartItem
                        {
                            ProductId = product.Id,
                            Quantity = addToCartDTO.Quantity, // أخدنا الكمية من الـ DTO بدل 1
                            Product = product,
                            ItemPrice = product.Price,
                            AddedOn = DateTime.UtcNow
                        });
                    }
                    addedCart = _unitOfWork.CartRepository.Update(cart);
                }
                await _unitOfWork.Complete();

                var result = _mapper.Map<CartItemsDTO>(addedCart);
                return new ServiceResult<CartItemsDTO>(result);
            }
            catch (Exception ex)
            {
                return new ServiceResult<CartItemsDTO>("Unexpected error happened: " + ex.Message, (int)StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ServiceResult<bool>> ClearCart(string userId)
        {
            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    return new ServiceResult<bool>("Cart not found", (int)StatusCodes.Status404NotFound);
                }
                cart.CartItems.Clear();
                _unitOfWork.CartRepository.Update(cart);
                await _unitOfWork.Complete();
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>("Unexpected error happend: " + ex.Message, (int)StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ServiceResult<CartItemsDTO>> GetCartItems(string userId)
        {
            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    return new ServiceResult<CartItemsDTO>(new CartItemsDTO());
                }
                var result = _mapper.Map<CartItemsDTO>(cart);
                return new ServiceResult<CartItemsDTO>(result);

            }
            catch (Exception ex)
            {
                return new ServiceResult<CartItemsDTO>("Unexpected error happend: " + ex.Message, (int)StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ServiceResult<CartItemsDTO>> RemoveItemFromCart(string userId,int productId)
        {
            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    return new ServiceResult<CartItemsDTO>("Cart not found", (int)StatusCodes.Status404NotFound);
                }
                var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
                if (cartItem == null)
                {
                    return new ServiceResult<CartItemsDTO>("Item was not found in the cart", (int)StatusCodes.Status404NotFound);
                }
                cart.CartItems.Remove(cartItem);
                var updatedCart = _unitOfWork.CartRepository.Update(cart);
                await _unitOfWork.Complete();
                var result = _mapper.Map<CartItemsDTO>(updatedCart);
                return new ServiceResult<CartItemsDTO>(result);

            }
            catch (Exception ex)
            {
                return new ServiceResult<CartItemsDTO>("Unexpected error happend: " + ex.Message, (int)StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ServiceResult<CartItemsDTO>> UpdateItemQuantity(string userId, int productId, int newQuantity)
        {
            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    return new ServiceResult<CartItemsDTO>("Cart not found", (int)StatusCodes.Status404NotFound);
                }
                var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
                if (cartItem == null)
                {
                    return new ServiceResult<CartItemsDTO>("Item was not found in the cart", (int)StatusCodes.Status404NotFound);
                }
                if (newQuantity > cartItem.Quantity)
                {
                    var product = await _unitOfWork.ProductRepository.GetById(productId);
                    if (product == null)
                    {
                        return new ServiceResult<CartItemsDTO>("Product not found", (int)StatusCodes.Status404NotFound);
                    }
                    if (product.Quantity < newQuantity)
                    {
                        return new ServiceResult<CartItemsDTO>("Not enough quantity in stock", (int)StatusCodes.Status400BadRequest);
                    }
                    
                }
                cartItem.Quantity = newQuantity;
                var updatedCart = _unitOfWork.CartRepository.Update(cart);
                await _unitOfWork.Complete();
                var result = _mapper.Map<CartItemsDTO>(updatedCart);
                return new ServiceResult<CartItemsDTO>(result);

            }
            catch (Exception ex)
            {
                return new ServiceResult<CartItemsDTO>("Unexpected error happend: " + ex.Message, (int)StatusCodes.Status500InternalServerError);

            }
        }
    }
    
    
}
