using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TMD_WalletMaster.Core.Services
{
    public class RegistrationResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; } = new List<IdentityError>();
    }
}