using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;

namespace E_Commerce.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response<Category>> AddCategory(AddCategoryDTO category)
        {
            var response = new Response<Category>();
            var newCategory = new Category { Name = category.CategoryName};
            var result = await unitOfWork.CategoryRepository.AddAsync(newCategory);

            try
            {
                await unitOfWork.Complete();
                response.Data = result;
                return response;
            }
            catch (Exception e)
            {
                response.Errors.Add(new Error {Code = 500, Message = "Error while saving Category: " + e.Message });
                return response;
            }
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await unitOfWork.CategoryRepository.GetAll();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await unitOfWork.CategoryRepository.GetById(id);
        }

        public async void DeleteCategory(int id)
        {
            var category = await GetCategory(id);
            unitOfWork.CategoryRepository.Remove(category);
        }



    }
}
