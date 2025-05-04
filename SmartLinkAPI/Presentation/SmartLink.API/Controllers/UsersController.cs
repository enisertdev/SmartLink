using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLink.Application.Authentication;
using SmartLink.Application.Repositories.User;
using SmartLink.Application.Services.Authentication;
using SmartLink.Application.Services.User;
using SmartLink.Domain.Entities;

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

        public class UserDTO
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }


        public UsersController(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IUserService userService, IUserAuthenticationService userAuthenticationService)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _userService = userService;
            _userAuthenticationService = userAuthenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            var userExists = await _userReadRepository.GetSingleAsync(u => u.Username == userDto.Username);
            if (userExists != null)
                return BadRequest(new { message = "A user already exists with this username." });
            if (userDto.Password.Count() < 6)
                return BadRequest(new { message = "Password cannot be shorter than 6 characters." });
            var hashedPassword = _userService.HashPassword(userDto.Password);
            UserEntity newUser = new() { Username = userDto.Username, PasswordHash = hashedPassword.Hash, PasswordSalt = hashedPassword.Salt };
            await _userWriteRepository.AddAsync(newUser);
            await _userWriteRepository.SaveChangesAsync();
            return Ok(newUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userReadRepository.GetAll(false);
            return Ok(users);
        }

        [Authorize]
        [HttpGet("Profile/{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var getUsername = User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            if (getUsername != username)
                return Forbid();
            var getUser = await _userReadRepository.GetSingleAsync(u => u.Username == username);
            return Ok(getUser);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserAuthentication user)
        {
            var token = await _userAuthenticationService.Login(user);
            if (token == null || token == string.Empty)
                return BadRequest(new { message = "Username or password is incorrect." });
            return Ok(new{token = token});
        }
    }
}
