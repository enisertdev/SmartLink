using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLink.Application.DTOs.User;
using SmartLink.Domain.Entities;

namespace SmartLink.Application.Services.User
{
    public interface IUserService
    {
        (string Hash, string Salt) HashPassword(string password);

        Task<bool> VerifyPassword(string username, string password, string salt);
        Task <bool> Login(UserLoginDTO user);
    }
}
