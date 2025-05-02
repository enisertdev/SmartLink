using SmartLink.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLink.Domain.Entities
{
    public class LinkTag : BaseEntity
    {
        public LinkEntity Link { get; set; }
        public Guid TagId { get; set; }
        public TagEntity Tag { get; set; }
    }
}
