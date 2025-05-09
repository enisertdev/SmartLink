using SmartLink.Application.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLink.Application.Services.Authentication
{
    public interface ITokenService
    {
        Task<string> GenerateToken(UserAuthentication model);
    }
}
