using System.Net;
using E_Commerce.Core.DTO.Authentication;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<Response<string>>> Register(RegisterDTO registerDTO)
        {
            var response = new Response<string>();
            var result = await _authenticationService.RegisterAsync(registerDTO);
            if (result.StatusCode == (int)HttpStatusCode.Conflict)
            {
                response.Errors.Add(new Error { Code = (int)HttpStatusCode.Conflict, Message = result.Message });
                return Conflict(response);
            }
            else if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code= (int)HttpStatusCode.InternalServerError, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            response.Data = result.Data;
            return Ok(response);

        }

        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<ActionResult<Response<string>>> ConfirmEmail(string email)
        {
            var response = new Response<string>();
            var result = await _authenticationService.ConfirmEmail(email);
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return NotFound(response);
            }
            else if (result.StatusCode == (int)HttpStatusCode.Conflict)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return Conflict(response);
            }
            else if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            response.Data = result.Data;
            return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<Response<AuthModel>>> Login(LoginDTO loginDTO)
        {
            var response = new Response<AuthModel>();
            var result = await _authenticationService.LoginAsync(loginDTO);
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
            {
                response.Errors.Add(new Error { Code = (int)HttpStatusCode.NotFound, Message = result.Message });
                return NotFound(response);
            }
            else if (result.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                response.Errors.Add(new Error { Code = (int)HttpStatusCode.Unauthorized, Message = result.Message });
                return Unauthorized(response);
            }
            else if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code = (int)HttpStatusCode.InternalServerError, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            response.Data = result.Data;

            if (!string.IsNullOrEmpty(result.Data.RefreshToken))
                SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpires);

            return Ok(response);
        }

        [HttpGet]
        [Route("GenerateNewTokenByRefreshToken")]
        public async Task<ActionResult<Response<AuthModel>>> GenerateNewTokenByRefreshToken()
        {
            var response = new Response<AuthModel>();
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authenticationService.GenerateNewTokenByRefreshTokenAsync(refreshToken);

            if (result.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                response.Errors.Add(new Error { Code = (int)HttpStatusCode.Unauthorized, Message = result.Message });
                return Unauthorized(response);
            }
            else if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code = (int)HttpStatusCode.InternalServerError, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            response.Data = result.Data;
            if (!string.IsNullOrEmpty(result.Data.RefreshToken))
                SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpires);
            return Ok(response);
        }

        [HttpPost]
        [Route("RevokeToken")]
        public async Task<ActionResult<Response<string>>> RevokeToken(RevokeTokenDTO token)
        {
            var response = new Response<string>();
            var refreshToken = token.RefreshToken ?? Request.Cookies["refreshToken"];
            var result = await _authenticationService.RevokeTokenAsync(refreshToken);
            if (result.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                response.Errors.Add(new Error { Code = (int)HttpStatusCode.Unauthorized, Message = result.Message });
                return Unauthorized(response);
            }
            else if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                response.Errors.Add(new Error { Code = (int)HttpStatusCode.InternalServerError, Message = result.Message });
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            response.Data = result.Data;
            return Ok(response);
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
