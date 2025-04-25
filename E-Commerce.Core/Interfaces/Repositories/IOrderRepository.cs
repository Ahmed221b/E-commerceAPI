using E_Commerce.Models;

namespace E_Commerce.Core.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        public Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId);
    }
}
