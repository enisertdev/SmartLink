using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLink.Application.Repositories.User;
using SmartLink.Domain.Entities;
using SmartLink.Persistance.DbContext;

namespace SmartLink.Persistance.Repositories.User
{
    public class UserReadRepository : ReadRepository<UserEntity>, IUserReadRepository
    {
        public UserReadRepository(SmartLinkDbContext context) : base(context)
        {
        }
    }
}
