using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Product;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IProductService
    {
        Task<ServiceResult<IEnumerable<GetProductDTO>>> GetAllProducts();
        Task<ServiceResult<GetProductDTO>> GetProductById(int id);
        Task<ServiceResult<GetProductDTO>> AddProduct(AddProductDTO product);
        Task<ServiceResult<GetProductDTO>> UpdateProduct(UpdateProductDTO product);
        Task<ServiceResult<bool>> DeleteProduct(int id);
        Task<ServiceResult<IEnumerable<GetProductDTO>>> GetProductsByCategory(int categoryId);
        Task<ServiceResult<IEnumerable<GetProductDTO>>> SearchProducts(string name);
        Task<ServiceResult<IEnumerable<GetProductDTO>>> FilterByPriceRange(double from,double to);



    }
}
