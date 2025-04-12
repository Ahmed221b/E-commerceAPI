using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Data;
using E_Commerce.EF.Repositories;

namespace E_Commerce.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
        }


        private ICategoryRepository _categoryRepository;
        public ICategoryRepository CategoryRepository
        {
            get => _categoryRepository == null ? _categoryRepository = new CategoryRepository(_context) : _categoryRepository;
        }

        private IColorRepository _colorRepository;
        public IColorRepository ColorRepository
        {
            get => _colorRepository == null ? _colorRepository = new ColorRepository(_context) : _colorRepository;
        }

        private IProductRepository _productRepository;
        public IProductRepository ProductRepository
        {
            get => _productRepository == null ? _productRepository = new ProductRepository(_context) : _productRepository;
        }

        private ICartRepository _cartRepository;
        public ICartRepository CartRepository
        {
            get => _cartRepository == null ? _cartRepository = new CartRepository(_context) : _cartRepository;
        }

        private IPaymentRepository _paymentRepository;
        public IPaymentRepository PaymentRepository
        {
            get => _paymentRepository == null ? _paymentRepository = new PaymentRepository(_context) : _paymentRepository;
        }

        private IOrderRepository _orderRepository;
        public IOrderRepository OrderRepository
        {
            get => _orderRepository == null ? _orderRepository = new OrderRepository(_context) : _orderRepository;
        }

        private IWishlistRepository _wishlistRepository;
        public IWishlistRepository WishlistRepository
        {
            get => _wishlistRepository == null ? _wishlistRepository = new WishlistRepository(_context) : _wishlistRepository;
        }
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }



        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
