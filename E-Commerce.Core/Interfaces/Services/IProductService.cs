using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Product;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<GetProductDTO>> GetAllProducts();
        Task<GetProductDTO> GetProductById(int id);
        Task<GetProductDTO> AddProduct(AddProductDTO product);
        Task<GetProductDTO> UpdateProduct(UpdateProductDTO product);
        Task<bool> DeleteProduct(int id);
        Task<IEnumerable<GetProductDTO>> GetProductsByCategory(int categoryId);
        Task<IEnumerable<GetProductDTO>> SearchProducts(string name);
        Task<IEnumerable<GetProductDTO>> FilterByPriceRange(double from,double to);



    }
}
