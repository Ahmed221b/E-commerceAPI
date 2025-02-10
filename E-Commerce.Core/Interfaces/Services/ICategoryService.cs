using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Category;
using E_Commerce.Core.Shared;
using E_Commerce.Models;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<GetCategoryDTO> AddCategory(CategoryDTO dto);
        Task<IEnumerable<GetCategoryListDTO>> GetCategories();
        Task<GetCategoryDTO> GetCategory(int id);
        Task<CategoryDTO> UpdateCategory(UpdateCategoryDTO dto);
        Task<bool> DeleteCategory(int id);
        Task<IEnumerable<GetCategoryDTO>> SearchCategories(string name);
    }
}
