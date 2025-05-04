using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLink.Application.Authentication;

namespace SmartLink.Application.Services.Authentication
{
    public interface IUserAuthenticationService
    {
        Task<string> Login(UserAuthentication model);
    }
}
