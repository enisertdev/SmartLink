using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;

namespace SmartLink.Application.Services.Authentication
{
    public interface IGoogleAuthService
    {
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string googleToken);
    }
}
