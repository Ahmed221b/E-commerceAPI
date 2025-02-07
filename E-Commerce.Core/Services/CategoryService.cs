using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        public async Task<Response<CategoryDTO>> AddCategory(CategoryDTO category)
        {
            var response = new Response<CategoryDTO>();
            if (await unitOfWork.CategoryRepository.AnyAsync(p => p.Name == category.CategoryName))
            {
                response.Errors.Add(new Error { Code = 409 ,Message = "A category wit the same name exists"});
                return response;
            }

            var newCategory = new Category { Name = category.CategoryName};
            var result = await unitOfWork.CategoryRepository.AddAsync(newCategory);

            try
            {
                await unitOfWork.Complete();
                response.Data = new CategoryDTO {CategoryName = result.Name };
                return response;
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error {Code = 500, Message = "Error while saving Category: " + ex.Message });
                return response;
            }
        }

        public async Task<Response<IEnumerable<GetCategoryDTO>>> GetCategories()
        {
            var response = new Response<IEnumerable<GetCategoryDTO>>();
            var categories = await unitOfWork.CategoryRepository.GetAll();
            if (categories == null)
            {
                response.Errors.Add(new Error { Code = 404, Message = "No Categories Found" });
                return response;
            }
            response.Data = categories.Select(P => new GetCategoryDTO { Id = P.Id, Name = P.Name });
            return response;
        }

        public async Task<Response<GetCategoryDTO>> GetCategory(int id)
        {
            var response = new Response<GetCategoryDTO>();
            var category = await unitOfWork.CategoryRepository.GetById(id);
            if (category == null)
            {
                response.Errors.Add(new Error { Code = 404, Message = "Category Not Found" });
                return response;
            }
            var responseData = new GetCategoryDTO { Id = category.Id, Name = category.Name };
            responseData.ProductsDto = category.Products
                .Select(p => new ProductDTO 
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Discount = p.Discount,
                    Image = p.Image,
                    Rate = p.Rate,
                }).ToList();
            response.Data = responseData;
            return response;
        }

        public async Task<Response<string>> DeleteCategory(int id)
        {
            var response = new Response<string>();
            var category = await GetCategory(id);
            if (category.Data == null)
            {
                response.Errors.Add(new Error { Code = 404, Message = "Category Not Found" });
                return response;
            }
            try
            {
                //unitOfWork.CategoryRepository.Remove(category.Data);
                await unitOfWork.Complete();
                response.Data = "Category Deleted Successfully";
                return response;
            }
            catch (Exception e)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Error while deleting Category: " + e.Message });
                return response;
            }

        }

        public async Task<Response<Category>> UpdateCategory(int id, CategoryDTO dto)
        {
            var response = new Response<Category>();
            var oldCategory = await GetCategory(id);
            if (oldCategory == null)
            {
                response.Errors.Add(new Error { Code = 404, Message = "Category Not Found" });
                return response;
            }
            try
            {
                //unitOfWork.CategoryRepository.Update(oldCategory.Data);
                await unitOfWork.Complete();
               // response.Data = oldCategory.Data;
                return response;
            }
            catch (Exception e)
            {
                response.Errors.Add(new Error { Code = 500, Message = "Error while updating Category: " + e.Message });
                return response;
            }

        }
    }
}
