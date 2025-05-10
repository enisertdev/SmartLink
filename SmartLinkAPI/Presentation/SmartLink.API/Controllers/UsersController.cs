using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartLink.Application.Authentication;
using SmartLink.Application.Repositories.User;
using SmartLink.Application.Services.Authentication;
using SmartLink.Application.Services.User;
using SmartLink.Domain.Entities;
using SmartLink.Domain.Entities.Identity;

namespace SmartLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUserService _userService;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly UserManager<AppUser> _userManager;

        public class UserDTO
        {
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }


        public UsersController(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IUserService userService, IUserAuthenticationService userAuthenticationService, UserManager<AppUser> userManager)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _userService = userService;
            _userAuthenticationService = userAuthenticationService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest(new { message = "A user already exists with this username." });

            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Email = model.Email,
                UserName = model.Username

            }, password: model.Password);

            if (result.Succeeded)
                return Ok(result);

            return BadRequest("Something went wrong. Try again later");
        }

        [Authorize]
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateUser()
        {
            var getUserIdFromToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(getUserIdFromToken))
                return Unauthorized();
            var user = await _userManager.FindByIdAsync(getUserIdFromToken);
            return Ok(new
            {
                IsValid = true,
                UserId = user.Id,
                Email = user.Email,
                Username = user.UserName
            });
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserAuthentication user)
        {
            try
            {
                var findUser = await _userManager.FindByEmailAsync(user.Email);
                var token = await _userAuthenticationService.Login(user);

                if (string.IsNullOrEmpty(token))
                    return BadRequest(new { message = "Username or password is incorrect." });

                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
