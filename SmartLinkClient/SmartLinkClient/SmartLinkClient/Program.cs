using System.Configuration;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using SmartLinkClient.Interfaces;
using SmartLinkClient.Services;

var builder = WebApplication.CreateBuilder(args);
var clientId = builder.Configuration["GoogleAuthentication:ClientId"];
var clientSecret = builder.Configuration["GoogleAuthentication:ClientSecret"];

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IVpnDetectorService, VpnDetectorService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(o =>
{
    o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogleOpenIdConnect(opt =>
    {
        opt.ClientId = clientId;
        opt.ClientSecret = clientSecret;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
