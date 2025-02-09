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
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }
    }
}
