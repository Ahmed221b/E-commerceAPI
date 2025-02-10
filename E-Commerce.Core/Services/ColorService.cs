using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Core.Custom_Exceptions;
using E_Commerce.Core.DTO.Color;
using E_Commerce.Core.Interfaces.Services;
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

        public async Task<GetColorDTO> CreateColor(ColorDTO color)
        {
            if(await _unitOfWork.ColorRepository.AnyAsync(p => p.Name == color.Name))
            {
                throw new ConflictException("A color with the same name already exists");
            }
            var newColor = new Color { Name = color.Name };
            await _unitOfWork.ColorRepository.AddAsync(newColor);
            await _unitOfWork.Complete();
            return _mapper.Map<GetColorDTO>(newColor);
        }

        public async Task<bool> DeleteColor(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetById(id);
            if (color == null)
                return false;

           _unitOfWork.ColorRepository.Remove(color);
            await _unitOfWork.Complete();
            return true;

        }

        public async Task<GetColorDTO> GetColor(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetById(id);
            if (color == null)
                return null;

            return _mapper.Map<GetColorDTO>(color);


        }

        public async Task<IEnumerable<GetColorDTO>> GetColors()
        {
            var colors = await _unitOfWork.ColorRepository.GetAll();
            return _mapper.Map<IEnumerable<GetColorDTO>>(colors);

        }

        public async Task<IEnumerable<GetColorDTO>> SearchColors(string name)
        {
            var colors = await _unitOfWork.ColorRepository.FindAsync(p => p.Name.Contains(name));

            return _mapper.Map<IEnumerable<GetColorDTO>>(colors);
        }

        public async Task<GetColorDTO> UpdateColor(UpdateColorDTO color)
        {
            if (await _unitOfWork.ColorRepository.AnyAsync(p => p.Name == color.ColorName))
            {
                throw new ConflictException("A color with the same name already exists");
            }
            var oldColor = await _unitOfWork.ColorRepository.GetById(color.Id);
            if (oldColor == null)
                return null;

            oldColor.Name = color.ColorName;
            var updatedColor = _unitOfWork.ColorRepository.Update(oldColor);
            await _unitOfWork.Complete();
            return _mapper.Map<GetColorDTO>(updatedColor);
        }
    }
}
