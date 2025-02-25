using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.Configuration;
using E_Commerce.Core.DTO.Authentication;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Models;
using E_Commerce.Core.Shared;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _JWT;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService _mailService;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor,IMailService mailService)
        {
            _userManager = userManager;
            _JWT = jwt.Value;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
        }

        public async Task<ServiceResult<string>> RegisterAsync(RegisterDTO registerDTO)
        {
            var model = new AuthModel();
            try
            {
                //if (await _userManager.FindByEmailAsync(registerDTO.Email) != null)
                //{
                //    model = new AuthModel("Email Already Exists");
                //    return new ServiceResult<string>(model.Message, (int)HttpStatusCode.Conflict);
                //}

                ApplicationUser newUser = GenerateApplicationUserObject(registerDTO);
                
                var result = await _userManager.CreateAsync(newUser, registerDTO.Password);
                if (!result.Succeeded)
                {
                    StringBuilder resultErrors = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        resultErrors.Append(error.Description + ", ");
                    }
                    model = new AuthModel(resultErrors.ToString());
                    return new ServiceResult<string>(model.Message, (int)HttpStatusCode.InternalServerError);
                }


                // Create the confirmation link
                var confirmationLink = _linkGenerator.GetUriByName(
                         _httpContextAccessor.HttpContext,
                         "ConfirmEmail",
                         new { Email = newUser.Email});

                var emailBody = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>";

                var serviceResult = await _mailService.SendEmailAsync(new List<string> { newUser.Email }, "Confirm your account", emailBody, true);

                if (serviceResult.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    model = new AuthModel(serviceResult.Message);
                    return new ServiceResult<string>(model.Message, (int)HttpStatusCode.InternalServerError);
                }

                return new ServiceResult<string>("User Registered Successfully, a mail was sent to you to confirm mail so you can login");
            }
            catch (Exception ex)
            {
                model = new AuthModel("Something Went Wrong " +ex.Message);
                return new ServiceResult<string>(model.Message,(int)HttpStatusCode.InternalServerError);
            }
           

        }

        public async Task<ServiceResult<string>> ConfirmEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new ServiceResult<string>("User Not Found", (int)HttpStatusCode.NotFound);
                }
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var result = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);

                if (result.Succeeded)
                {
                    return new ServiceResult<string>("Email Confirmed Successfully");
                }
                else
                {
                    StringBuilder resultErrors = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        resultErrors.Append(error.Description + ", ");
                    }
                    return new ServiceResult<string>(resultErrors.ToString(), (int)HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>("Something Went Wrong " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        private ApplicationUser GenerateApplicationUserObject(RegisterDTO registerDTO)
        {
            if (!registerDTO.IsAdmin)
            {
                return new Customer
                {
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    Email = registerDTO.Email,
                    Address = registerDTO.Address ?? string.Empty,
                    PhoneNumber = registerDTO.PhoneNumber,
                    DataOfBirth = registerDTO.DateOfBirth ?? DateOnly.FromDateTime(DateTime.Now.Date),
                    UserName = registerDTO.FirstName.ToLower() + registerDTO.LastName.ToLower() + new Random().Next(1000, 9999)
                };
            }
            else
            {
                return new Admin
                {
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    Email = registerDTO.Email,
                    PhoneNumber = registerDTO.PhoneNumber,
                    UserName = registerDTO.FirstName.ToLower() + registerDTO.LastName.ToLower() + new Random().Next(1000, 9999)
                };

            }
        }


        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var rolesClaims = new List<Claim>();
            foreach (var role in roles)
            {
                rolesClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(rolesClaims);

            var key = Environment.GetEnvironmentVariable("JWT_KEY");
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _JWT.Issuer,
                audience: _JWT.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_JWT.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken =  Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = DateTime.Now.AddDays(10),
                CreatedOn = DateTime.Now,
            };
        }

        
    }
}
