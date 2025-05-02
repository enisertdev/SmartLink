using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLink.Domain.Entities;
using SmartLink.Domain.Entities.Common;

namespace SmartLink.Application.DTOs.Link
{
    public class CreateLinkDTO : BaseEntity
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Summary { get; set; }
        public List<string> Tags { get; set; }
    }
}
