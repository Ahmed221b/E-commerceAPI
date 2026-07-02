using E_Commerce.Core.DTO.Role;
using E_Commerce.Core.Shared;

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
        Task<ServiceResult<IEnumerable<UserAutoCompleteDTO>>> SearchUsersToAssign(string query);
    }
}
