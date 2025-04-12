using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Core.Models;
using E_Commerce.Data;

namespace E_Commerce.EF.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDBContext _context;
        public PaymentRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }


}
