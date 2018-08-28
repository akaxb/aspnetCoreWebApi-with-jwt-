using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Services
{
    public class AuthCodeService : IAuthCodeService
    {
        public bool Validate(string phone, string code)
        {
            return true;
        }
    }
}
