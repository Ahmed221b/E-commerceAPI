using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Role;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<ServiceResult<IEnumerable<GetRoleDTO>>> GetAllRoles();
        Task<ServiceResult<GetRoleDTO>> GetRoleById(string id);
        Task<ServiceResult<GetRoleDTO>> GetRoleByName(string roleName);
        Task<ServiceResult<GetRoleDTO>> AddRole(string roleName);
        Task<ServiceResult<GetRoleDTO>> UpdateRole(string oldroleName,string newRoleName);
        Task<ServiceResult<bool>> DeleteRole(string roleName);
        Task<ServiceResult<string>> AssignUserToRole(UserRoleDTO addUserToRoleDTO);
        Task<ServiceResult<string>> RemoveUserFromRole(UserRoleDTO addUserToRoleDTO);

    }
}
