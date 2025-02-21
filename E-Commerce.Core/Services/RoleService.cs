using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Core.DTO.Role;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ServiceResult<GetRoleDTO>> AddRole(string roleName)
        {
            try
            {
                if (await _roleManager.RoleExistsAsync(roleName))
                {
                    return new ServiceResult<GetRoleDTO>("Role already exists", (int)HttpStatusCode.Conflict);
                }
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                var usersInRole = new List<ApplicationUser>();
                if (result.Succeeded)
                {
                    usersInRole = (List<ApplicationUser>)await _userManager.GetUsersInRoleAsync(roleName);
                }
                var date = new GetRoleDTO
                {
                    RoleName = roleName,
                    usersInRoleDTOs = _mapper.Map<List<UsersInRoleDTO>>(usersInRole)
                };
                return new ServiceResult<GetRoleDTO>(date);
            }
            catch (Exception e)
            {
                return new ServiceResult<GetRoleDTO>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<bool>> DeleteRole(string roleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                
                if (role == null)
                {
                    return new ServiceResult<bool>("Role not found", (int)HttpStatusCode.NotFound);
                }
                var result = await _roleManager.DeleteAsync(role);
                return new ServiceResult<bool>(result.Succeeded);
            }
            catch(Exception e)
            {
                return new ServiceResult<bool>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<IEnumerable<GetRoleDTO>>> GetAllRoles()
        {
            try
            {
                var result = await _roleManager.Roles.ToListAsync();
                if (result.Count == 0)
                {
                    return new ServiceResult<IEnumerable<GetRoleDTO>>("No roles found", (int)HttpStatusCode.NotFound);
                }
                var roles = _mapper.Map<IEnumerable<GetRoleDTO>>(result);
                foreach (var role in roles)
                {
                    role.usersInRoleDTOs = _mapper.Map<List<UsersInRoleDTO>>(await _userManager.GetUsersInRoleAsync(role.RoleName));
                }
                return new ServiceResult<IEnumerable<GetRoleDTO>>(roles);
            }
            catch (Exception e)
            {
                return new ServiceResult<IEnumerable<GetRoleDTO>>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<GetRoleDTO>> GetRoleById(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return new ServiceResult<GetRoleDTO>($"Role with {id} not found", (int)HttpStatusCode.NotFound);
                }

                var date = _mapper.Map<GetRoleDTO>(role);
                date.usersInRoleDTOs = _mapper.Map<List<UsersInRoleDTO>>(await _userManager.GetUsersInRoleAsync(role.Name));
                return new ServiceResult<GetRoleDTO>(date);
            }
            catch (Exception e)
            {
                return new ServiceResult<GetRoleDTO>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<GetRoleDTO>> GetRoleByName(string roleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    return new ServiceResult<GetRoleDTO>($"Role {roleName} not found", (int)HttpStatusCode.NotFound);
                }

                var date = _mapper.Map<GetRoleDTO>(role);
                date.usersInRoleDTOs = _mapper.Map<List<UsersInRoleDTO>>(await _userManager.GetUsersInRoleAsync(roleName));
                return new ServiceResult<GetRoleDTO>(date);
            }
            catch (Exception e)
            {
                return new ServiceResult<GetRoleDTO>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<GetRoleDTO>> UpdateRole(string oldroleName, string newRoleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(oldroleName);
                if (role == null)
                {
                    return new ServiceResult<GetRoleDTO>($"Role {oldroleName} not found", (int)HttpStatusCode.NotFound);
                }
                role.Name = newRoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    return new ServiceResult<GetRoleDTO>("Something went wrong,Role not updated", (int)HttpStatusCode.InternalServerError);
                }
                var date = _mapper.Map<GetRoleDTO>(role);
                date.usersInRoleDTOs = _mapper.Map<List<UsersInRoleDTO>>(await _userManager.GetUsersInRoleAsync(newRoleName));
                return new ServiceResult<GetRoleDTO>(date);
            }
            catch (Exception e)
            {
                return new ServiceResult<GetRoleDTO>(e.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
