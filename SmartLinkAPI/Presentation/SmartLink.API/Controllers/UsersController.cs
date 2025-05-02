using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLink.Application.Repositories.User;
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

        public class UserForCreate
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public UsersController(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IUserService userService)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserForCreate user)
        {
            var hashedPassword = _userService.HashPassword(user.Password);
            UserEntity newUser = new() { Username = user.Username, PasswordHash = hashedPassword.Hash,PasswordSalt = hashedPassword.Salt};
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
    }
}
