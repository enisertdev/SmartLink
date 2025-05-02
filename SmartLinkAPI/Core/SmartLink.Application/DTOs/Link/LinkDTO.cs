using SmartLink.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLink.Application.DTOs.Link
{
    public class LinkDTO : BaseEntity
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Summary { get; set; }  
        public DateTime CreatedAt { get; set; }
        public bool IsArchived { get; set; }

        public List<string> Tags { get; set; }
    }
}
