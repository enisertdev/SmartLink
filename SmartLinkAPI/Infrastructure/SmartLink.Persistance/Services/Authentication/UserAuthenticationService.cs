using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartLink.Application.Authentication;
using SmartLink.Application.Repositories.User;
using SmartLink.Application.Services.Authentication;
using SmartLink.Application.Services.User;

namespace SmartLink.Persistance.Services.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ITokenService _tokenService;

        public UserAuthenticationService(IConfiguration configuration, IUserService userService, IUserReadRepository userReadRepository, ITokenService tokenService)
        {
            _configuration = configuration;
            _userService = userService;
            _userReadRepository = userReadRepository;
            _tokenService = tokenService;
        }

        public async Task<string> Login(UserAuthentication model)
        {
            var getUser = await _userReadRepository.GetSingleAsync(u => u.Username == model.Username);
            if (getUser == null)
                return string.Empty;
            var loginUser = await _userService.VerifyPassword(model.Username, model.Password, getUser.PasswordSalt);
            if (!loginUser)
                return string.Empty;
            return _tokenService.GenerateToken(model);
        }
    }
}
