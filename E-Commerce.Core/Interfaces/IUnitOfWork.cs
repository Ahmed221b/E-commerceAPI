using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.Interfaces.Repositories;

namespace E_Commerce.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {

        //Add The Repositories of the appliction here
        ICategoryRepository CategoryRepository { get; }
        IColorRepository ColorRepository { get; }
        IProductRepository ProductRepository { get; }
        ICartRepository CartRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IOrderRepository OrderRepository { get; }
        IWishlistRepository WishlistRepository { get; }
        IReviewRepository ReviewRepository { get; }
        Task<int> Complete();
    }
}
