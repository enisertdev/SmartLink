using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartLink.Application.Repositories.Link;
using SmartLink.Application.Repositories.Tag;
using SmartLink.Application.Repositories.User;
using SmartLink.Application.Services;
using SmartLink.Application.Services.Authentication;
using SmartLink.Application.Services.User;
using SmartLink.Domain.Entities.Identity;
using SmartLink.Persistance.DbContext;
using SmartLink.Persistance.Repositories.Link;
using SmartLink.Persistance.Repositories.Tag;
using SmartLink.Persistance.Repositories.User;
using SmartLink.Persistance.Services;
using SmartLink.Persistance.Services.Authentication;
using SmartLink.Persistance.Services.User;

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
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<ILinkService, LinkService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<HttpClient>();
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<SmartLinkDbContext>();
        }
    }
}
