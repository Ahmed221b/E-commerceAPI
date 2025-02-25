using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace E_Commerce.Core.Shared
{
    public class AuthModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
        public bool IsAuthenticated { get; set; }
        
        public string? Message { get; set; }

        public AuthModel()
        {
            
        }
        public AuthModel(string message)
        {
            Message = message;
            IsAuthenticated = false;
        }

    }
}
