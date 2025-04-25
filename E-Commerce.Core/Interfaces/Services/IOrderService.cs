
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Order;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IOrderService
    {

        Task<ServiceResult<GetOrderDTO>> CreateOrderAsync(string CustomerId);
        Task<ServiceResult<GetOrderDTO>> GetOrderByIdAsync(int orderId);
        Task<ServiceResult<List<GetOrderDTO>>> GetOrdersByCustomerIdAsync(string customerId);
        Task<ServiceResult<List<GetOrderDTO>>> GetAllOrdersAsync();
        Task<ServiceResult<GetOrderDTO>> CancelOrder(int orderId);
        Task<ServiceResult<List<GetOrderDTO>>> GetOrdersByStatusAsync(string status);
        Task<ServiceResult<GetOrderDTO>> UpdateOrderStatusAsync(int orderId, string status);



    }
}
