using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Core.Custom_Exceptions;
using E_Commerce.Core.DTO.Color;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;

namespace E_Commerce.Core.Services
{
    public class ColorService : IColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ColorService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<GetColorDTO>> CreateColor(AddColorDTO color)
        {
            try
            {
                if (await _unitOfWork.ColorRepository.AnyAsync(p => p.Name == color.Name))
                    return new ServiceResult<GetColorDTO>("A color with the same name already exists", 409);

                var newColor = new Color { Name = color.Name };
                await _unitOfWork.ColorRepository.AddAsync(newColor);
                await _unitOfWork.Complete();
                var data = _mapper.Map<GetColorDTO>(newColor);
                return new ServiceResult<GetColorDTO>(data);
            }
            catch (Exception ex)
            {
                return new ServiceResult<GetColorDTO>(ex.Message, 500);
            }
   
        }

        public async Task<ServiceResult<bool>> DeleteColor(int id)
        {
            try
            {
                var color = await _unitOfWork.ColorRepository.GetById(id);
                if (color == null)
                    return new ServiceResult<bool>($"No color with id = {id} found", 404);

                _unitOfWork.ColorRepository.Remove(color);
                await _unitOfWork.Complete();
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex.Message, 500);
            }

        }

        public async Task<ServiceResult<GetColorDTO>> GetColor(int id)
        {
            try
            {
                var color = await _unitOfWork.ColorRepository.GetById(id);
                if (color == null)
                    return new ServiceResult<GetColorDTO>($"No color with id = {id} found", 404);

                var data = _mapper.Map<GetColorDTO>(color);
                return new ServiceResult<GetColorDTO>(data);
            }
            catch (Exception ex)
            {
                return new ServiceResult<GetColorDTO>(ex.Message, 500);
            }
        }

        public async Task<ServiceResult<IEnumerable<GetColorDTO>>> GetColors()
        {
            try
            {
                var colors = await _unitOfWork.ColorRepository.GetAll();
                if (colors.Count() == 0)
                    return new ServiceResult<IEnumerable<GetColorDTO>>("No colors found", 404);
                var data = _mapper.Map<IEnumerable<GetColorDTO>>(colors);
                return new ServiceResult<IEnumerable<GetColorDTO>>(data);
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<GetColorDTO>>(ex.Message, 500);
            }
            

        }

        public async Task<ServiceResult<IEnumerable<GetColorDTO>>> SearchColors(string name)
        {
            try
            {
                var colors = await _unitOfWork.ColorRepository.FindAsync(p => p.Name.StartsWith(name));
                if (colors.Count() == 0)
                    return new ServiceResult<IEnumerable<GetColorDTO>>("No match found", 404);
                var data = _mapper.Map<IEnumerable<GetColorDTO>>(colors);
                return new ServiceResult<IEnumerable<GetColorDTO>>(data);
            }
            catch (Exception ex)
            {
                return new ServiceResult<IEnumerable<GetColorDTO>>(ex.Message,500);
            }
           
        }

        public async Task<ServiceResult<GetColorDTO>> UpdateColor(UpdateColorDTO color)
        {
            try
            {
                if (await _unitOfWork.ColorRepository.AnyAsync(p => p.Name == color.ColorName))
                    return new ServiceResult<GetColorDTO>("A color with the same name already exists", 409);
                var oldColor = await _unitOfWork.ColorRepository.GetById(color.Id);
                if (oldColor == null)
                    return new ServiceResult<GetColorDTO>($"No color with id {color.Id} found", 404);

                oldColor.Name = color.ColorName;
                var updatedColor = _unitOfWork.ColorRepository.Update(oldColor);
                await _unitOfWork.Complete();
                var data = _mapper.Map<GetColorDTO>(updatedColor);
                return new ServiceResult<GetColorDTO>(data);
            }
            catch (Exception ex)
            {
                return new ServiceResult<GetColorDTO>(ex.Message, 500);
            }
        }
    }
}
