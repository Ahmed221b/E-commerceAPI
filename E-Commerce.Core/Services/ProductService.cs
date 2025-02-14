using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using E_Commerce.Core.DTO.Color;
using E_Commerce.Core.DTO.Product;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ServiceResult<GetProductDTO>> AddProduct(AddProductDTO productDto)
        {
            try
            {
                var newProduct = _mapper.Map<Product>(productDto);
                foreach (var colorId in productDto.ColorIds)
                {
                    var color = await _unitOfWork.ColorRepository.GetById(colorId);
                    if (color != null)
                        newProduct.ProductColors.Add(new ProductColor { Color = color, Product = newProduct});
                }
                newProduct.Category = await _unitOfWork.CategoryRepository.GetById(productDto.CategoryId);
                var addedProduct = await _unitOfWork.ProductRepository.AddAsync(newProduct);
                await _unitOfWork.Complete();
                var data = _mapper.Map<GetProductDTO>(addedProduct);
                return new ServiceResult<GetProductDTO>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<GetProductDTO>(e.Message,(int)HttpStatusCode.InternalServerError);
            }

        }

        public async Task<ServiceResult<bool>> DeleteProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetById(id);
                if (product == null)
                    return new ServiceResult<bool>($"No product with id {id} was found",(int)HttpStatusCode.NotFound);
                
                _unitOfWork.ProductRepository.Remove(product);
                await _unitOfWork.Complete();
                return new ServiceResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ServiceResult<bool>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<IEnumerable<GetProductDTO>>> FilterByPriceRange(double from, double to)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.FindAsync(p => p.Price >= from && p.Price <= to );
                if (products.Count() == 0)
                    return new ServiceResult<IEnumerable<GetProductDTO>>("No match found", (int)HttpStatusCode.NotFound);

                var data = _mapper.Map<IEnumerable<GetProductDTO>>(products);
                return new ServiceResult<IEnumerable<GetProductDTO>>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<IEnumerable<GetProductDTO>>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<IEnumerable<GetProductDTO>>> GetAllProducts()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAll();
                if (products.Count() == 0)
                    return new ServiceResult<IEnumerable<GetProductDTO>>("No products were found", (int)HttpStatusCode.NotFound);

                var data = _mapper.Map<IEnumerable<GetProductDTO>>(products);
                return new ServiceResult<IEnumerable<GetProductDTO>>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<IEnumerable<GetProductDTO>>(e.Message,(int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<GetProductDTO>> GetProductById(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetById(id);
                if (product == null)
                    return new ServiceResult<GetProductDTO>($"No product with id {id} was found", (int)HttpStatusCode.NotFound);

                var data = _mapper.Map<GetProductDTO>(product);
                //data.Colors = product.ProductColors.Select(c => c.Color.Name).ToList();
                return new ServiceResult<GetProductDTO>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<GetProductDTO>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<IEnumerable<GetProductDTO>>> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.FindAsync(p => p.CategoryId == categoryId);
                if (products.Count() == 0)
                    return new ServiceResult<IEnumerable<GetProductDTO>>("No Products found for this category", (int)HttpStatusCode.NotFound);

                var data = _mapper.Map<IEnumerable<GetProductDTO>>(products);
                return new ServiceResult<IEnumerable<GetProductDTO>>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<IEnumerable<GetProductDTO>>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<IEnumerable<GetProductDTO>>> SearchProducts(string name)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.FindAsync(p => p.Name.StartsWith(name));
                if (products.Count() == 0)
                    return new ServiceResult<IEnumerable<GetProductDTO>>("No match found", (int)HttpStatusCode.NotFound);

                var data = _mapper.Map<IEnumerable<GetProductDTO>>(products);
                return new ServiceResult<IEnumerable<GetProductDTO>>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<IEnumerable<GetProductDTO>>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<GetProductDTO>> UpdateProduct(UpdateProductDTO newProduct)
        {
            try
            {
                // Fetch the existing product
                var product = await _unitOfWork.ProductRepository.GetById(newProduct.Id);
                if (product == null)
                    return new ServiceResult<GetProductDTO>($"No product with id {newProduct.Id} was found", (int)HttpStatusCode.NotFound);

                // Update properties if provided
                if (!string.IsNullOrEmpty(newProduct.NewName))
                    product.Name = newProduct.NewName;
                if (newProduct.NewPrice.HasValue)
                    product.Price = newProduct.NewPrice.Value;
                if (!string.IsNullOrEmpty(newProduct.NewDescription))
                    product.Description = newProduct.NewDescription;
                if (newProduct.NewDiscount.HasValue)
                    product.Discount = newProduct.NewDiscount.Value;
                if (!string.IsNullOrEmpty(newProduct.NewImageBase64))
                    product.Image = Convert.FromBase64String(newProduct.NewImageBase64);
                if (newProduct.NewQuantity.HasValue)
                    product.Quantity = newProduct.NewQuantity.Value;

                // Update Category if provided
                if (newProduct.NewCategoryId.HasValue)
                {
                    var category = await _unitOfWork.CategoryRepository.GetById(newProduct.NewCategoryId.Value);
                    if (category == null)
                        return new ServiceResult<GetProductDTO>($"No category with id {newProduct.NewCategoryId.Value} was found", (int)HttpStatusCode.NotFound);
                    product.Category = category;
                }

                // Update ProductColors if provided
                if (newProduct.NewColorIds != null && newProduct.NewColorIds.Any())
                {
                    // Clear existing ProductColors
                    product.ProductColors.Clear();

                    // Add new ProductColors
                    foreach (var colorId in newProduct.NewColorIds)
                    {
                        var color = await _unitOfWork.ColorRepository.GetById(colorId);
                        if (color == null)
                            return new ServiceResult<GetProductDTO>($"No color with id {colorId} was found", (int)HttpStatusCode.NotFound);
                        product.ProductColors.Add(new ProductColor { Color = color, Product = product });
                    }
                }

                // Update the product in the database
                var updated = _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.Complete();

                // Map the updated product to GetProductDTO
                var data = _mapper.Map<GetProductDTO>(updated);
                return new ServiceResult<GetProductDTO>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<GetProductDTO>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
