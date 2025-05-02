using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SmartLink.Persistance.DbContext;

namespace SmartLink.Persistance
{
    public class SmartListContextFactory : IDesignTimeDbContextFactory<SmartLinkDbContext>
    {
        public SmartLinkDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SmartLinkDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            return new (optionsBuilder.Options);
        }
    }
}
