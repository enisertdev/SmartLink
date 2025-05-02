using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLink.Application.Repositories.Tag;
using SmartLink.Domain.Entities;
using SmartLink.Persistance.DbContext;

namespace SmartLink.Persistance.Repositories.Tag
{
    public class TagReadRepository : ReadRepository<TagEntity>, ITagReadRepository
    {
        public TagReadRepository(SmartLinkDbContext context) : base(context)
        {
        }
    }
}
