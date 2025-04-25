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
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<CommonResponse<string>>> Register(RegisterDTO registerDTO)
        {
            var response = new CommonResponse<string>();
            var result = await _authenticationService.RegisterAsync(registerDTO);
            if (result.StatusCode == (int)HttpStatusCode.Created)
            {
                response.Data = result.Data;
                return StatusCode(StatusCodes.Status201Created, response);
            }
            else
            {
                response.Errors.Add(new Error { Code = result.StatusCode, Message = result.Message });
                return StatusCode(result.StatusCode, response);
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<CommonResponse<AuthModel>>> Login(LoginDTO loginDTO)
        {
            var response = new CommonResponse<AuthModel>();
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

        // GET: api/auth/users/{email}/confirm
        [HttpGet("users/{email}/confirm")]
        [AllowAnonymous]
        public async Task<ActionResult<CommonResponse<string>>> ConfirmEmail(string email)
        {
            var response = new CommonResponse<string>();
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

        // POST: api/auth/tokens/refresh
        [HttpPost("tokens/refresh")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CommonResponse<AuthModel>>> GenerateNewTokenByRefreshToken()
        {
            var response = new CommonResponse<AuthModel>();
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

        // POST: api/auth/tokens/revoke
        [HttpPost("tokens/revoke")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CommonResponse<string>>> RevokeToken(RevokeTokenDTO token)
        {
            var response = new CommonResponse<string>();
            var refreshToken = token.RefreshToken ?? Request.Cookies["refreshToken"];
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            var accessToken = authHeader?.Split(" ").Last();
            var result = await _authenticationService.RevokeTokenAsync(refreshToken, accessToken);

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

        // PUT: api/auth/users/password
        [HttpPut("users/password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CommonResponse<string>>> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var response = new CommonResponse<string>();
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

        // POST: api/auth/users/password/forgot
        [HttpPost("users/password/forgot")]
        [AllowAnonymous]
        public async Task<ActionResult<CommonResponse<string>>> ForgotPassword(string email)
        {
            var response = new CommonResponse<string>();
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

        // GET: api/auth/users/password/reset-data
        [HttpGet("users/password/reset-data")]
        [AllowAnonymous]
        public IActionResult ResetPasswordData(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Invalid reset link" });
            }

            return Ok(new { email, token });
        }

        // POST: api/auth/users/password/reset
        [HttpPost("users/password/reset")]
        [AllowAnonymous]
        public async Task<ActionResult<CommonResponse<string>>> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var response = new CommonResponse<string>();
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