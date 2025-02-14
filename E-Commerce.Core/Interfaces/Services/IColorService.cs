using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Color;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IColorService
    {
        Task<ServiceResult<IEnumerable<ColorDTO>>> GetColors();
        Task<ServiceResult<ColorDTO>> GetColor(int id);
        Task<ServiceResult<ColorDTO>> CreateColor(AddColorDTO color);
        Task<ServiceResult<ColorDTO>> UpdateColor(UpdateColorDTO color);
        Task<ServiceResult<bool>> DeleteColor(int id);
        Task<ServiceResult<IEnumerable<ColorDTO>>> SearchColors(string name);


    }
}
