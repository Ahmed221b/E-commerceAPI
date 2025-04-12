
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Data;
using E_Commerce.Models;

namespace E_Commerce.EF.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDBContext _context;
        public OrderRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
