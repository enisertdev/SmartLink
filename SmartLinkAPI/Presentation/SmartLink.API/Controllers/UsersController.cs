using System.Security.Claims;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartLink.Application.Authentication;
using SmartLink.Application.DTOs.User;
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
        private readonly IValidator<UserRegisterDto> _validator;


        public UsersController(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IUserService userService, IUserAuthenticationService userAuthenticationService, UserManager<AppUser> userManager, IValidator<UserRegisterDto> validator)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _userService = userService;
            _userAuthenticationService = userAuthenticationService;
            _userManager = userManager;
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterDto model)
        {
            var validatorResult = await _validator.ValidateAsync(model);
            if (!validatorResult.IsValid)
                return BadRequest(new{message =validatorResult.Errors.Select(e => e.ErrorMessage).First()});


            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest(new { message = "An user with this email already exists." });


            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Email = model.Email,
                UserName = model.Username

            }, password: model.Password);

            if (result.Succeeded)
                return Ok(new{message = "Register was successful.You can now login."});

            return BadRequest(new {message = result.Errors.Select(e => e.Description).FirstOrDefault()});
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

                return Ok(new { token = token , message = "Login Successful."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
