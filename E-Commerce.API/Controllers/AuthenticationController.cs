using System.Net;
using E_Commerce.Core.DTO.Authentication;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code= result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmail")]
        public async Task<ActionResult<Response<string>>> ConfirmEmail(string email)
        {
            var response = new Response<string>();
            var result = await _authenticationService.ConfirmEmail(email);
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
        [Route("Login")]
        public async Task<ActionResult<Response<AuthModel>>> Login(LoginDTO loginDTO)
        {
            var response = new Response<AuthModel>();
            var result = await _authenticationService.LoginAsync(loginDTO);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;

                if (!string.IsNullOrEmpty(result.Data.RefreshToken))
                    SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpires);
                return Ok(response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        [HttpGet]
        [Route("GenerateNewTokenByRefreshToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Response<AuthModel>>> GenerateNewTokenByRefreshToken()
        {
            var response = new Response<AuthModel>();
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authenticationService.GenerateNewTokenByRefreshTokenAsync(refreshToken);

            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                if (!string.IsNullOrEmpty(result.Data.RefreshToken))
                    SetRefreshTokenInCookie(result.Data.RefreshToken, result.Data.RefreshTokenExpires);
                return Ok(response);
            }
            else 
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
            
        }

        [HttpPost]
        [Route("RevokeToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Response<string>>> RevokeToken(RevokeTokenDTO token)
        {
            var response = new Response<string>();
            var refreshToken = token.RefreshToken ?? Request.Cookies["refreshToken"];
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            var accessToken = authHeader?.Split(" ").Last();
            var result = await _authenticationService.RevokeTokenAsync(refreshToken, accessToken);
            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                response.Data = result.Data;
                return Ok(response); ;
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
            
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<ActionResult<Response<string>>> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var response = new Response<string>();
            var result = await _authenticationService.ChangePassword(changePasswordDTO);
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
        [Route("ForgotPassword")]
        public async Task<ActionResult<Response<string>>> ForgotPassword(string email)
        {
            var response = new Response<string>();
            var result = await _authenticationService.ForgotPassword(email);
            if (result.StatusCode == (int)StatusCodes.Status200OK)
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
        [Route("ResetPasswordData", Name = "ResetPasswordData")]
        public IActionResult ResetPasswordData(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Invalid reset link" });
            }

            return Ok(new { email, token });
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<ActionResult<Response<string>>> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var response = new Response<string>();
            var result = await _authenticationService.ResetPassword(resetPasswordDTO);
            if (result.StatusCode == (int)StatusCodes.Status200OK)
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
