using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Selfcare.Authentication;

namespace eSim.Infrastructure.Interfaces.Selfcare.Authentication
{
    public interface ISignIn
    {
       public Task<bool> AuthenticateAsync(SignIn model);

        public bool IsAuthenticated();

        public string GetToken();
    }
}
