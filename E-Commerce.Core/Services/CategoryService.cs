using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Xml.Linq;
using AutoMapper;
using E_Commerce.Core.Custom_Exceptions;
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
        private readonly IMapper mapper;
        public CategoryService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<CategoryDTO> AddCategory(CategoryDTO category)
        {
            if (await unitOfWork.CategoryRepository.AnyAsync(p => p.Name == category.CategoryName))
            {
                throw new ConflictException("A category with the same name exists.");
            }

            var newCategory = new Category { Name = category.CategoryName};
            var result = await unitOfWork.CategoryRepository.AddAsync(newCategory);
            await unitOfWork.Complete();
            return new CategoryDTO { CategoryName = result.Name };
        }
        public async Task<IEnumerable<GetCategoryListDTO>> GetCategories()
        {

            var categories = await unitOfWork.CategoryRepository.GetAll();
            if (categories == null)
            {
                return null;
            }
            return mapper.Map<IEnumerable<GetCategoryListDTO>>(categories);
        }
        public async Task<GetCategoryDTO> GetCategory(int id)
        {
            var category = await unitOfWork.CategoryRepository.GetById(id);
            if (category == null)
            {
                return null;
            }

            var responseData = new GetCategoryDTO { Id = category.Id, Name = category.Name };
            responseData.ProductsDto = mapper.Map<List<ProductDTO>>(category.Products);
            return responseData;
        }
        public async Task<CategoryDTO> UpdateCategory(int id, CategoryDTO dto)
        {
            if (await unitOfWork.CategoryRepository.AnyAsync(p => p.Name == dto.CategoryName))
            {
                throw new ConflictException("A category with the same name exists");
            }
            var oldCategory = await unitOfWork.CategoryRepository.GetById(id);
            if (oldCategory == null)
            {
                return null;
            }
            oldCategory.Name = dto.CategoryName;
            var result = unitOfWork.CategoryRepository.Update(oldCategory);
            await unitOfWork.Complete();
            return new CategoryDTO { CategoryName = result.Name };
        }
        public async Task<string> DeleteCategory(int id)
        {
            var category = await unitOfWork.CategoryRepository.GetById(id);
            if (category == null)
                return null;
            unitOfWork.CategoryRepository.Remove(category);
            await unitOfWork.Complete();
            return "Category Deleted Successfully";
        }

        public async Task<IEnumerable<GetCategoryDTO>> SearchCategories(string name)
        {
            var result = await unitOfWork.CategoryRepository.FindAsync(p => p.Name.StartsWith(name));
            if (result == null)
            {
                return null;
            }
            return mapper.Map<IEnumerable<GetCategoryDTO>>(result);
        }
    }
}
