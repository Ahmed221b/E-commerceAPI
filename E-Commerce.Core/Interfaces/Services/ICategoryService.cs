using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO;
using E_Commerce.Core.Shared;
using E_Commerce.Models;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<Response<CategoryDTO>> AddCategory(CategoryDTO dto);
        Task<Response<IEnumerable<GetCategoryDTO>>> GetCategories();
        Task<Response<GetCategoryDTO>> GetCategory(int id);
        Task<Response<Category>> UpdateCategory(int id, CategoryDTO dto);
    }
}
