using SmartLink.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLink.Domain.Entities
{
    public class TagEntity : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<LinkTag> LinkTags { get; set; }
    }
}
