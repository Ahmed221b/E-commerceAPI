using System.Net;
using AutoMapper;
using E_Commerce.Core.DTO.Order;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;

namespace E_Commerce.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResult<GetOrderDTO>> CreateOrderAsync(string CustomerId)
        {
            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(CustomerId);
                if (cart == null || !cart.CartItems.Any())
                {
                    return new ServiceResult<GetOrderDTO>("Cart is Empty!", (int)HttpStatusCode.NotFound);
                }
                var newOrder = await _unitOfWork.OrderRepository.AddAsync(
                    new Order
                    {
                        Status = Constants.OrderStatus.Pending.ToString(),
                        OrderDate = DateTime.UtcNow,
                        TotalPrice = cart.TotalPrice,
                        CustomerId = CustomerId,
                        OrderProducts = cart.CartItems.Select(cp => new OrderProduct
                        {
                            ProductId = cp.ProductId,
                            Quantity = cp.Quantity,
                            Price = cp.ItemPrice
                        }).ToList()

                    }
                );
                await HandleStock(newOrder);
                await _unitOfWork.Complete();

                var result = _mapper.Map<GetOrderDTO>(newOrder);
                return new ServiceResult<GetOrderDTO>(result);
            }
            catch (Exception ex)
            {
                return new ServiceResult<GetOrderDTO>("Unexpected error: " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }


        private async Task HandleStock(Order order)
        {

            foreach (var item in order.OrderProducts)
            {
                var product = await _unitOfWork.ProductRepository.GetById(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                    _unitOfWork.ProductRepository.Update(product);
                }
            }
        
        }
    }
}
