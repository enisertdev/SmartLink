using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLink.Domain.Entities;

namespace SmartLink.Application.Repositories.User
{
    public interface IUserReadRepository : IReadRepository<UserEntity>
    {
    }
}
