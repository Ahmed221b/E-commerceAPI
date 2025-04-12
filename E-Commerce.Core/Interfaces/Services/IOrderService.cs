
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Order;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IOrderService
    {

        Task<ServiceResult<GetOrderDTO>> CreateOrderAsync(string CustomerId,int paymentRecord);
        //Task<ServiceResult<GetOrderDTO>> GetOrderByIdAsync(int orderId);
        //Task<ServiceResult<List<GetOrderDTO>>> GetOrdersByCustomerIdAsync(string customerId);



    }
}
