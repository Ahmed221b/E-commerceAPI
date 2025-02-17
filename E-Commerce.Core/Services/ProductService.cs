using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Core.DTO.Product;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;

namespace E_Commerce.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<ServiceResult<GetProductDTO>> AddProduct(AddProductDTO product)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<bool>> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<GetProductDTO>>> FilterByPriceRange(double from, double to)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<GetProductDTO>>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<GetProductDTO>> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<GetProductDTO>>> GetProductsByCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<GetProductDTO>>> SearchProducts(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<GetProductDTO>> UpdateProduct(UpdateProductDTO product)
        {
            throw new NotImplementedException();
        }
    }
}