﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Category
{
    public class GetCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CategpryProductDTO> ProductsDto { get; set; } = new List<CategpryProductDTO>();

    }
}
