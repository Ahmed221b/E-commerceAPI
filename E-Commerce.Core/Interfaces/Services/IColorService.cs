using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Color;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IColorService
    {
        Task<IEnumerable<GetColorDTO>> GetColors();
        Task<GetColorDTO> GetColor(int id);
        Task<GetColorDTO> CreateColor(ColorDTO color);
        Task<GetColorDTO> UpdateColor(UpdateColorDTO color);
        Task<bool> DeleteColor(int id);
        Task<IEnumerable<GetColorDTO>> SearchColors(string name);


    }
}
