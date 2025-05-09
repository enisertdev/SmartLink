using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartLink.Application.Authentication;
using SmartLink.Application.Repositories.User;
using SmartLink.Application.Services.Authentication;
using SmartLink.Application.Services.User;
using SmartLink.Domain.Entities.Identity;

namespace SmartLink.Persistance.Services.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;

        public UserAuthenticationService(IConfiguration configuration, IUserService userService, IUserReadRepository userReadRepository, ITokenService tokenService, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userService = userService;
            _userReadRepository = userReadRepository;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<string> Login(UserAuthentication model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return string.Empty;
            var loginUser =await _userManager.CheckPasswordAsync(user, model.Password);
            if (!loginUser)
                return string.Empty;
            return await _tokenService.GenerateToken(model);
        }
    }
}
