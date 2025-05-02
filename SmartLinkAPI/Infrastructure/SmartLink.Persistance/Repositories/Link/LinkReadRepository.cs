using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLink.Application.Repositories.Link;
using SmartLink.Domain.Entities;
using SmartLink.Persistance.DbContext;

namespace SmartLink.Persistance.Repositories.Link
{
    public class LinkReadRepository : ReadRepository<LinkEntity>,ILinkReadRepository
    {
        public LinkReadRepository(SmartLinkDbContext context) : base(context)
        {
        }
    }
}
