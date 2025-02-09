using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Data;
using E_Commerce.Models;

namespace E_Commerce.EF.Repositories
{
    public class ColorRepository : BaseRepository<Color>, IColorRepository
    {
        private readonly ApplicationDBContext _context;
        public ColorRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }
    }
}
