using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartLink.Domain.Entities;

namespace SmartLink.Persistance.DbContext
{
    public class SmartLinkDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<LinkEntity> Links { get; set; }
        public DbSet<LinkTag> LinkTags { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public SmartLinkDbContext(DbContextOptions<SmartLinkDbContext> options) : base(options)
        {

        }
    }
}
