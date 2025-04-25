using System.Net;
using AutoMapper;
using E_Commerce.Core.DTO.Order;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentGatewayService _paymentGatewayService;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentGatewayService paymentGatewayService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentGatewayService = paymentGatewayService;
        }

        public async Task<ServiceResult<GetOrderDTO>> CancelOrder(int orderId)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetById(orderId);
                if (order == null)
                {
                    return new ServiceResult<GetOrderDTO>("Order not found", StatusCodes.Status404NotFound);
                }
                if (order.Status != Constants.OrderStatus.Processing.ToString())
                {
                    return new ServiceResult<GetOrderDTO>($"Order cannot be cancelled it's {order.Status}", StatusCodes.Status400BadRequest);
                }
                order.Status = Constants.OrderStatus.Cancelled.ToString();
                await HandleStock(order, false);
                var refundResult = await _paymentGatewayService.RefundPayment(order.Payment.PaymentIntentId);
                if (refundResult == null || refundResult.Data.RefundStatus != "succeeded")
                {
                    return new ServiceResult<GetOrderDTO>("Refund failed", StatusCodes.Status500InternalServerError);
                }
                order.Payment.PaymentStatus = Constants.PaymentStatus.Refunded.ToString();
                _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.Complete();
                var result = _mapper.Map<GetOrderDTO>(order);
                return new ServiceResult<GetOrderDTO>(result);

            }
            catch (Exception ex)
            {
                return new ServiceResult<GetOrderDTO>("Unexpected error: " + ex.Message,  StatusCodes.Status500InternalServerError);
            }
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
                        Status = Constants.OrderStatus.Processing.ToString(),
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
                await HandleStock(newOrder,true);
                await _unitOfWork.Complete();

                var result = _mapper.Map<GetOrderDTO>(newOrder);
                return new ServiceResult<GetOrderDTO>(result);
            }
            catch (Exception ex)
            {
                return new ServiceResult<GetOrderDTO>("Unexpected error: " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<List<GetOrderDTO>>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetAll();
                if (orders == null || !orders.Any())
                {
                    return new ServiceResult<List<GetOrderDTO>>("No orders found", (int)HttpStatusCode.NotFound);
                }
                var result = _mapper.Map<List<GetOrderDTO>>(orders);
                return new ServiceResult<List<GetOrderDTO>>(result);
            }
            catch(Exception ex)
            {
                return new ServiceResult<List<GetOrderDTO>>("Unexpected error: " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<GetOrderDTO>> GetOrderByIdAsync(int orderId)
        {

            try
            {
                var order = await _unitOfWork.OrderRepository.GetById(orderId);
                if (order == null)
                {
                    return new ServiceResult<GetOrderDTO>("Order not found", StatusCodes.Status404NotFound);
                }
                var result = _mapper.Map<GetOrderDTO>(order);
                return new ServiceResult<GetOrderDTO>(result);
            }
            catch (Exception ex)
            {
                return new ServiceResult<GetOrderDTO>("Unexpected error: " + ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ServiceResult<List<GetOrderDTO>>> GetOrdersByCustomerIdAsync(string customerId)
        {
            try
            {
                var customerOrders = await _unitOfWork.OrderRepository.GetOrdersByCustomerIdAsync(customerId);
                if (!customerOrders.Any())
                    return new ServiceResult<List<GetOrderDTO>>("No orders found for this customer", StatusCodes.Status404NotFound);

                var result = _mapper.Map<List<GetOrderDTO>>(customerOrders);
                return new ServiceResult<List<GetOrderDTO>>(result);
            }
            catch(Exception ex)
            {
                return new ServiceResult<List<GetOrderDTO>>("Unexpected error: " + ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public Task<ServiceResult<List<GetOrderDTO>>> GetOrdersByStatusAsync(string status)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<GetOrderDTO>> UpdateOrderStatusAsync(int orderId, string status)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetById(orderId);
                if (order == null)
                {
                    return new ServiceResult<GetOrderDTO>("Order not found", StatusCodes.Status404NotFound);
                }
      
                order.Status = status;
                if (status == Constants.OrderStatus.Rejected.ToString())
                {
                    await HandleStock(order, false);
                    var refundResult = await _paymentGatewayService.RefundPayment(order.Payment.PaymentIntentId);
                    if (refundResult == null || refundResult.Data.RefundStatus != "succeeded")
                    {
                        return new ServiceResult<GetOrderDTO>("Refund failed", StatusCodes.Status500InternalServerError);
                    }
                    order.Payment.PaymentStatus = Constants.PaymentStatus.Refunded.ToString();

                }
                _unitOfWork.OrderRepository.Update(order);
                await _unitOfWork.Complete();
                var result = _mapper.Map<GetOrderDTO>(order);
                return new ServiceResult<GetOrderDTO>(result);

            }
            catch (Exception ex)
            {
                return new ServiceResult<GetOrderDTO>("Unexpected error: " + ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task HandleStock(Order order,bool deduct)
        {

            foreach (var item in order.OrderProducts)
            {
                var product = await _unitOfWork.ProductRepository.GetById(item.ProductId);
                if (product != null)
                {
                    if (deduct)
                        product.Quantity -= item.Quantity;
                    else
                        product.Quantity += item.Quantity;
                    
                    _unitOfWork.ProductRepository.Update(product);
                }
            }
        
        }
    }
}
