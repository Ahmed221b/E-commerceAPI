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
        Task<ServiceResult<GetCategoryDTO>> AddCategory(AddCategoryDTO dto);
        Task<ServiceResult<IEnumerable<GetCategoryListDTO>>> GetCategories();
        Task<ServiceResult<GetCategoryDTO>> GetCategory(int id);
        Task<ServiceResult<GetCategoryDTO>> UpdateCategory(UpdateCategoryDTO dto);
        Task<ServiceResult<bool>> DeleteCategory(int id);
        Task<ServiceResult<IEnumerable<GetCategoryListDTO>>> SearchCategories(string name);
    }
}
