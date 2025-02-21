using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Role
{
    public class GetRoleDTO
    {
        public string RoleName { get; set; }
        public List<UsersInRoleDTO> usersInRoleDTOs { get; set; }
    }
}
