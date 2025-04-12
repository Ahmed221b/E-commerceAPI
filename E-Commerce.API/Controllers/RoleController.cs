using System.Net;
using E_Commerce.Core.DTO.Role;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        [Route(nameof(AddRole))]
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
                return StatusCode(result.StatusCode,response);
            }
       
        }

        [HttpGet]
        [Route(nameof(GetAllRoles))]
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

        [HttpGet]
        [Route(nameof(GetRoleByName))]
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

        [HttpGet]
        [Route(nameof(GetRoleById))]
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

        [HttpDelete]
        [Route(nameof(DeleteRole))]
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
        [Route(nameof(UpdateRole))]
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

        [HttpPost]
        [Route(nameof(AssignUserToRole))]
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

        [HttpPost]
        [Route(nameof(RemoveUserFromRole))]
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
