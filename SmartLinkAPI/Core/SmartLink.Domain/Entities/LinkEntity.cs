using SmartLink.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLink.Domain.Entities
{
    public class LinkEntity : BaseEntity
    {
        public string Url { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Summary { get; set; }
        public bool IsArchived { get; set; } = false;
        public string? Notes { get; set; }
        public Guid? UserId { get; set; }
        public UserEntity? User { get; set; }
        public ICollection<LinkTag> LinkTags { get; set; }
    }
}
