using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.DTO.Authentication;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<ServiceResult<string>> RegisterAsync(RegisterDTO registerDTO);
        Task<ServiceResult<string>> ConfirmEmail(string email);
        Task<ServiceResult<AuthModel>> LoginAsync(LoginDTO loginDTO);
        Task<ServiceResult<AuthModel>> GenerateNewTokenByRefreshTokenAsync (string refreshToken);
        Task<ServiceResult<string>> RevokeTokenAsync (string refreshToken);
    }
}
