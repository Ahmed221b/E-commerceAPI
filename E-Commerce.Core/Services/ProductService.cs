using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Product;
using E_Commerce.Core.Interfaces.Services;

namespace E_Commerce.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<GetProductDTO> AddProduct(AddProductDTO product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetProductDTO>> FilterByPriceRange(double from, double to)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetProductDTO>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<GetProductDTO> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetProductDTO>> GetProductsByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetProductDTO>> SearchProducts(string name)
        {
            throw new NotImplementedException();
        }

        public Task<GetProductDTO> UpdateProduct(UpdateProductDTO product)
        {
            throw new NotImplementedException();
        }
    }
}
