using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartLink.Application.Repositories.Link;
using SmartLink.Application.Repositories.Tag;
using SmartLink.Application.Services;
using SmartLink.Persistance.DbContext;
using SmartLink.Persistance.Repositories.Link;
using SmartLink.Persistance.Repositories.Tag;
using SmartLink.Persistance.Services;

namespace SmartLink.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            services.AddDbContext<SmartLinkDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));
            services.AddScoped<ILinkReadRepository, LinkReadRepository>();
            services.AddScoped<ILinkWriteRepository, LinkWriteRepository>();
            services.AddScoped<ITagReadRepository, TagReadRepository>();
            services.AddScoped<ITagWriteRepository, TagWriteRepository>();
            services.AddScoped<ILinkService, LinkService>();
        }
    }
}
