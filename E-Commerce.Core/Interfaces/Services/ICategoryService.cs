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
        Task<CategoryDTO> AddCategory(CategoryDTO dto);
        Task<IEnumerable<GetCategoryListDTO>> GetCategories();
        Task<GetCategoryDTO> GetCategory(int id);
        Task<CategoryDTO> UpdateCategory(int id, CategoryDTO dto);
        Task<string> DeleteCategory(int id);
        Task<IEnumerable<GetCategoryDTO>> SearchCategories(string name);
    }
}
