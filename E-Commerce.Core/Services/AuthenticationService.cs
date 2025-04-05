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
                if (await _userManager.FindByEmailAsync(registerDTO.Email) != null)
                {
                    model = new AuthModel("Email Already Exists");
                    return new ServiceResult<string>(model.Message, (int)HttpStatusCode.Conflict);
                }

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
                    return new ServiceResult<string>(serviceResult.Message, (int)HttpStatusCode.InternalServerError);
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
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    return new ServiceResult<string>("Email is already confirmed", (int)HttpStatusCode.Conflict);
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
        public async Task<ServiceResult<AuthModel>> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password))
                {
                    return new ServiceResult<AuthModel>("Invalid Email Or Password", (int)HttpStatusCode.Unauthorized);
                }
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return new ServiceResult<AuthModel>("Please confirm your email first", (int)HttpStatusCode.Unauthorized);
                }
                var token = await GenerateToken(user);
                var authModel = new AuthModel
                {
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Token = token,
                    IsAuthenticated = true
                };

                //If there is an active refresh token, return it, else generate a new one
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(p => p.IsActive);
                if (activeRefreshToken != null)
                {
                    authModel.RefreshToken = activeRefreshToken.Token;
                    authModel.RefreshTokenExpires = activeRefreshToken.ExpiresOn;
                }
                else
                {
                    var refreshToken = GenerateRefreshToken();
                    user.RefreshTokens.Add(refreshToken);
                    authModel.RefreshToken = refreshToken.Token;
                    authModel.RefreshTokenExpires = refreshToken.ExpiresOn;
                }
                await _userManager.UpdateAsync(user);

                return new ServiceResult<AuthModel>(authModel);

            }
            catch (Exception ex)
            {
                return new ServiceResult<AuthModel>("Something Went Wrong " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<AuthModel>> GenerateNewTokenByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(u => u.Token == refreshToken));
                if (user == null)
                {
                    return new ServiceResult<AuthModel>("Invalid Refresh Token", (int)HttpStatusCode.Unauthorized);
                }

                var oldRefreshToken = user.RefreshTokens.Single(u => u.Token == refreshToken);
                if (!oldRefreshToken.IsActive)
                {
                    return new ServiceResult<AuthModel>("Refresh Token Expired", (int)HttpStatusCode.Unauthorized);
                }

                oldRefreshToken.RevokedOn = DateTime.UtcNow;
                var newJWT = await GenerateToken(user);
                var newRefreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);

                return new ServiceResult<AuthModel>(new AuthModel
                {
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Token = newJWT,
                    IsAuthenticated = true,
                    RefreshToken = newRefreshToken.Token,
                    RefreshTokenExpires = newRefreshToken.ExpiresOn
                });

            }
            catch (Exception ex)
            {
                return new ServiceResult<AuthModel>("Something Went Wrong " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<string>> RevokeTokenAsync(string refreshToken,string accessToken)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(u => u.Token == refreshToken));
                if (user == null)
                {
                    return new ServiceResult<string>("Invalid Refresh Token", (int)HttpStatusCode.Unauthorized);
                }
                var token = user.RefreshTokens.Single(u => u.Token == refreshToken);
                if (!token.IsActive)
                {
                    return new ServiceResult<string>("Refresh Token Expired", (int)HttpStatusCode.Unauthorized);
                }
                token.RevokedOn = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
               
                return new ServiceResult<string>("Token Revoked Successfully");
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>("Something Went Wrong " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<string>> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(changePasswordDTO.Email);
                if (user == null)
                {
                    return new ServiceResult<string>("User Not Found", (int)HttpStatusCode.NotFound);
                }
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, changePasswordDTO.CurrentPassword);
                if (!isPasswordValid)
                {
                    return new ServiceResult<string>("Invalid Old Password", (int)HttpStatusCode.Unauthorized);
                }

                var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);
                if (result.Succeeded)
                {
                    user.RefreshTokens.Clear();
                    await _userManager.UpdateAsync(user);
                    return new ServiceResult<string>("Password Changed Successfully");
                }
                else
                {
                    StringBuilder resultErrors = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        resultErrors.Append(error.Description + ", ");
                    }
                    return new ServiceResult<string>("Changing password failed: " + resultErrors.ToString(), (int)HttpStatusCode.InternalServerError);
                }
            }
            catch(Exception ex)
            {
                return new ServiceResult<string>("Something Went Wrong " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<string>> ForgotPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new ServiceResult<string>("User Not Found", (int)HttpStatusCode.NotFound);
                }
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = _linkGenerator.GetUriByName(
                         _httpContextAccessor.HttpContext,
                         "ResetPasswordData",
                         new { Email = user.Email, Token = resetToken });
                var emailBody = $"Please reset your password by clicking this link: <a href='{resetLink}'>link</a>";
                var serviceResult = await _mailService.SendEmailAsync(new List<string> { user.Email },"Reset Password", emailBody, true);
                if (serviceResult.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    return new ServiceResult<string>(serviceResult.Message, (int)HttpStatusCode.InternalServerError);
                }
                return new ServiceResult<string>("A link was sent to your mail to reset password");
            }
            catch(Exception ex)
            {
                return new ServiceResult<string>("Something Went Wrong " + ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ServiceResult<string>> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
                if (user == null)
                {
                    return new ServiceResult<string>("User not found", (int)HttpStatusCode.NotFound);
                }

                var resetResult = await _userManager.ResetPasswordAsync(user, resetPasswordDTO.ResetToken, resetPasswordDTO.NewPassword);

                if (!resetResult.Succeeded)
                {
                    StringBuilder errors = new StringBuilder();
                    foreach (var error in resetResult.Errors)
                    {
                        errors.Append(error.Description + ", ");
                    }
                    return new ServiceResult<string>("Reset password failed: " + errors.ToString(), (int)HttpStatusCode.InternalServerError);
                }

                return new ServiceResult<string>("Password reset successfully.");
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>("Something went wrong: " + ex.Message, (int)HttpStatusCode.InternalServerError);
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
                new Claim(JwtRegisteredClaimNames.Sub, user.Id), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
        
                // .NET specific claims
                new Claim(ClaimTypes.NameIdentifier, user.Id), 
                new Claim("username", user.UserName)
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
