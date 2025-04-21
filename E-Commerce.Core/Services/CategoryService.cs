using AutoMapper;
using E_Commerce.Core.DTO.Category;
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

        public async Task<ServiceResult<GetCategoryDTO>> AddCategory(AddCategoryDTO category)
        {
            if (await unitOfWork.CategoryRepository.AnyAsync(p => p.Name == category.CategoryName))
            {
                return new ServiceResult<GetCategoryDTO>("A category with the same name exists", 409);
            }
            try
            {
                var newCategory = new Category { Name = category.CategoryName };
                var result = await unitOfWork.CategoryRepository.AddAsync(newCategory);
                await unitOfWork.Complete();
                var data = mapper.Map<GetCategoryDTO>(result);
                return new ServiceResult<GetCategoryDTO>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<GetCategoryDTO>(e.Message, 500);
            }
          
        }
        public async Task<ServiceResult<IEnumerable<GetCategoryListDTO>>> GetCategories()
        {
            try
            {
                var categories = await unitOfWork.CategoryRepository.GetAll();
                if (categories.Count() == 0)
                    return new ServiceResult<IEnumerable<GetCategoryListDTO>>("No Categories found", 404);
                
                var data = mapper.Map<IEnumerable<GetCategoryListDTO>>(categories);
                return new ServiceResult<IEnumerable<GetCategoryListDTO>>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<IEnumerable<GetCategoryListDTO>>(e.Message, 500);
            }
            
        }
        public async Task<ServiceResult<GetCategoryDTO>> GetCategory(int id)
        {
            try
            {
                var category = await unitOfWork.CategoryRepository.GetById(id);
                if (category == null)
                    return new ServiceResult<GetCategoryDTO>($"No Category with id {id} was found", 404);

                var data = mapper.Map<GetCategoryDTO>(category);
                data.ProductsDto = mapper.Map<List<CategpryProductDTO>>(category.Products);
                return new ServiceResult<GetCategoryDTO>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<GetCategoryDTO>(e.Message, 500);
            }

        }

        public async Task<ServiceResult<GetCategoryDTO>> UpdateCategory(UpdateCategoryDTO dto)
        {
            try
            {
                if (await unitOfWork.CategoryRepository.AnyAsync(p => p.Name == dto.CategoryName))
                {
                    return new ServiceResult<GetCategoryDTO>("A category with the same name exists", 409);
                }
                var oldCategory = await unitOfWork.CategoryRepository.GetById(dto.Id);
                if (oldCategory == null)
                    return new ServiceResult<GetCategoryDTO>($"No Category with id {dto.Id} was found", 404);

                oldCategory.Name = dto.CategoryName;
                var result = unitOfWork.CategoryRepository.Update(oldCategory);
                await unitOfWork.Complete();
                var updatedCategory = mapper.Map<GetCategoryDTO>(result);
                return new ServiceResult<GetCategoryDTO>(updatedCategory);
            }
            catch (Exception e)
            {
                return new ServiceResult<GetCategoryDTO>(e.Message, 500);
            }

        }

        public async Task<ServiceResult<bool>> DeleteCategory(int id)
        {
            try
            {
                var category = await unitOfWork.CategoryRepository.GetById(id);
                if (category == null)
                    return new ServiceResult<bool>($"No Category with id {id} was found", 404);
                unitOfWork.CategoryRepository.Remove(category);
                await unitOfWork.Complete();
                return new ServiceResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ServiceResult<bool>(e.Message, 500);
            }

        }

        public async Task<ServiceResult<IEnumerable<GetCategoryListDTO>>> SearchCategories(string name)
        {
            try
            {
                var result = await unitOfWork.CategoryRepository.FindAsync(p => p.Name.StartsWith(name));
                if (result.Count() == 0)
                    return new ServiceResult<IEnumerable<GetCategoryListDTO>>("No match found!", 404);
                var data = mapper.Map<IEnumerable<GetCategoryListDTO>>(result);
                return new ServiceResult<IEnumerable<GetCategoryListDTO>>(data);
            }
            catch (Exception e)
            {
                return new ServiceResult<IEnumerable<GetCategoryListDTO>>(e.Message, 500);
            }
        }
    }
}
