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
        public async Task<ActionResult<Response<GetRoleDTO>>> AddRole(string roleName)
        {
            var response = new Response<GetRoleDTO>();
            var result = await _roleService.AddRole(roleName);
            if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code = result.StatusCode,Message = "Unexpected error happend "+result.Message});
                return StatusCode(StatusCodes.Status500InternalServerError,response);
            }
            if (result.StatusCode == (int)HttpStatusCode.Conflict)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return Conflict(response);
            }
            response.Data = result.Data;
            return Ok(response);
        }

        [HttpGet]
        [Route(nameof(GetAllRoles))]
        public async Task<ActionResult<Response<IEnumerable<GetRoleDTO>>>> GetAllRoles()
        {
            var response = new Response<IEnumerable<GetRoleDTO>>();
            var result = await _roleService.GetAllRoles();
            if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = "Unexpected error happend " + result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = "No roles found" });
                return NotFound(response);
            }
            response.Data = result.Data;
            return Ok(response);
        }

        [HttpGet]
        [Route(nameof(GetRoleByName))]
        public async Task<ActionResult<Response<GetRoleDTO>>> GetRoleByName(string roleName)
        {
            var response = new Response<GetRoleDTO>();
            var result = await _roleService.GetRoleByName(roleName);
            if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = "Unexpected error happend " + result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return NotFound(response);
            }
            response.Data = result.Data;
            return Ok(response);
        }

        [HttpGet]
        [Route(nameof(GetRoleById))]
        public async Task<ActionResult<Response<GetRoleDTO>>> GetRoleById(string id)
        {
            var response = new Response<GetRoleDTO>();
            var result = await _roleService.GetRoleById(id);
            if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = "Unexpected error happend " + result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return NotFound(response);
            }
            response.Data = result.Data;
            return Ok(response);
        }

        [HttpDelete]
        [Route(nameof(DeleteRole))]
        public async Task<ActionResult<Response<string>>> DeleteRole(string roleName)
        {
            var response = new Response<string>();
            var result = await _roleService.DeleteRole(roleName);
            if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = "Unexpected error happend " + result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return NotFound(response);
            }
            response.Data = "Role deleted Successfully";
            return Ok(response);
        }

        [HttpPut]
        [Route(nameof(UpdateRole))]
        public async Task<ActionResult<Response<GetRoleDTO>>> UpdateRole(string oldRoleName,string newRoleName)
        {
            var response = new Response<GetRoleDTO>();
            var result = await _roleService.UpdateRole(oldRoleName,newRoleName);
            if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = "Unexpected error happend " + result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return NotFound(response);
            }
            response.Data = result.Data;
            return Ok(response);
        }
    }
}
