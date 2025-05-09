using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartLink.Domain.Entities;
using SmartLink.Domain.Entities.Identity;

namespace SmartLink.Persistance.DbContext
{
    public class SmartLinkDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public DbSet<LinkEntity> Links { get; set; }
        public DbSet<LinkTag> LinkTags { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        // public DbSet<UserEntity> Users { get; set; }
        public SmartLinkDbContext(DbContextOptions<SmartLinkDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var keysProperties = builder.Model.GetEntityTypes().Select(x => x.FindPrimaryKey()).SelectMany(x => x.Properties);
            foreach (var property in keysProperties)
            {
                property.ValueGenerated = ValueGenerated.OnAdd;
            }
        }
    }
}
