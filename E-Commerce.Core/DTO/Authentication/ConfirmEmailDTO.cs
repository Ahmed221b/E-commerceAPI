using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Authentication
{
    public class ConfirmEmailDTO
    {
        public string Email { get; set; }
        public string ConfirmationToken { get; set; }
    }
}
