
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartLink.Application.DTOs.User;
using SmartLink.Application.Repositories.User;
using SmartLink.Application.Services.User;
using SmartLink.Domain.Entities;

namespace SmartLink.Persistance.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserReadRepository _userReadRepository;

        public UserService(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public (string Hash, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(128 / 8);
            string salt = Convert.ToBase64String(saltBytes);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));
            return (hashed, salt);
        }

        public async Task<bool> VerifyPassword(string username, string password, string salt)
        {
            UserEntity user = await _userReadRepository.GetSingleAsync(u => u.Username == username);
            if (user == null)
            {
                return false;

            }
            byte[] saltBytes = Convert.FromBase64String(salt);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));

            return user.PasswordHash == hash;

        }

        public async Task<bool> Login(UserLoginDTO user)
        {
            var getUser = await _userReadRepository.GetSingleAsync(u => u.Username == user.Username);
            if (getUser == null)
                return false;

            var verifyUser = await VerifyPassword(user.Username, user.Password, getUser.PasswordSalt);
            if (!verifyUser)
                return false;

            return true;

        }
    }
}
