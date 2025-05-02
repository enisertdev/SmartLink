
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SmartLink.Application.Services.User;

namespace SmartLink.Persistance.Services.User
{
    public class UserService : IUserService
    {
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
    }
}
