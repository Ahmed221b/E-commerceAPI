using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Authentication
{
    public class RegisterDTO
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }

        [RegularExpression(@"^(\+20|0)?1[0-2,5]{1}[0-9]{8}$",ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        [Compare("Password",ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool IsAdmin { get; set; } = false; 

    }
}
