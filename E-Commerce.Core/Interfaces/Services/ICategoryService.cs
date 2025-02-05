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
        Task<Response<Category>> AddCategory(CategoryDTO dto);
        Task<Response<IEnumerable<Category>>> GetCategories();
        Task<Response<Category>> GetCategory(int id);
        Task<Response<Category>> UpdateCategory(int id, CategoryDTO dto);
    }
}
