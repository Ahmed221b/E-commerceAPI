
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Order;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IOrderService
    {

        Task<ServiceResult<GetOrderDTO>> CreateOrderAsync(string CustomerId);
        



    }
}
