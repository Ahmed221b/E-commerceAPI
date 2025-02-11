using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Core.DTO.Product;
using E_Commerce.Core.Interfaces.Services;
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

        public async Task<GetProductDTO> AddProduct(AddProductDTO product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
            
            var addedProduct = await _unitOfWork.ProductRepository.AddAsync(newProduct);
            await _unitOfWork.Complete();
            return _mapper.Map<GetProductDTO>(addedProduct);
        }

        public Task<bool> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetProductDTO>> FilterByPriceRange(double from, double to)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GetProductDTO>> GetAllProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetAll();
            return _mapper.Map<IEnumerable<GetProductDTO>>(products);
        }

        public async Task<GetProductDTO> GetProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetById(id);
            return _mapper.Map<GetProductDTO>(product);
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
