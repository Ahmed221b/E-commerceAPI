
using E_Commerce.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
