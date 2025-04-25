using System.Net;
using E_Commerce.Core.DTO.Role;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Constants.Admin},{Constants.Supervisor}")]

    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ActionResult<CommonResponse<GetRoleDTO>>> AddRole(string roleName)
        {
            var response = new CommonResponse<GetRoleDTO>();
            var result = await _roleService.AddRole(roleName);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }

        }

        [HttpGet]
        public async Task<ActionResult<CommonResponse<IEnumerable<GetRoleDTO>>>> GetAllRoles()
        {
            var response = new CommonResponse<IEnumerable<GetRoleDTO>>();
            var result = await _roleService.GetAllRoles();
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpGet("by-name/{roleName}")]
        public async Task<ActionResult<CommonResponse<GetRoleDTO>>> GetRoleByName(string roleName)
        {
            var response = new CommonResponse<GetRoleDTO>();
            var result = await _roleService.GetRoleByName(roleName);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommonResponse<GetRoleDTO>>> GetRoleById(string id)
        {
            var response = new CommonResponse<GetRoleDTO>();
            var result = await _roleService.GetRoleById(id);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpDelete("{roleName}")]
        public async Task<ActionResult<CommonResponse<string>>> DeleteRole(string roleName)
        {
            var response = new CommonResponse<string>();
            var result = await _roleService.DeleteRole(roleName);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = "Role Deleted Successfully";
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpPut]
        public async Task<ActionResult<CommonResponse<GetRoleDTO>>> UpdateRole(string oldRoleName,string newRoleName)
        {
            var response = new CommonResponse<GetRoleDTO>();
            var result = await _roleService.UpdateRole(oldRoleName,newRoleName);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpPost("add-users-to-role")]
        public async Task<ActionResult<CommonResponse<string>>> AssignUserToRole(UserRoleDTO addUserToRoleDTO)
        {
            var response = new CommonResponse<string>();
            var result = await _roleService.AssignUserToRole(addUserToRoleDTO);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Message;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpPost("remove-users-from-role")]
        public async Task<ActionResult<CommonResponse<string>>> RemoveUserFromRole(UserRoleDTO addUserToRoleDTO)
        {
            var response = new CommonResponse<string>();
            var result = await _roleService.RemoveUserFromRole(addUserToRoleDTO);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Message;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }
    }
}
