using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Core.Services
{
    public interface IEmailVerificationService
    {
        Task<bool>IsValidEmailAsync(string email);
    }
}
