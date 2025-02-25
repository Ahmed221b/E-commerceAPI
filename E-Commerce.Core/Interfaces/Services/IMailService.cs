using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Core.Shared;

namespace E_Commerce.Core.Interfaces.Services
{
    public interface IMailService
    {
        Task<ServiceResult<bool>> SendEmailAsync(List<string> emails, string subject, string message, bool isBodyHtml);
    }
}
