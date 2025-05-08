
using System.Text.Json;
using SmartLinkClient.Interfaces;
using SmartLinkClient.Models;

namespace SmartLinkClient.Services
{
    public class VpnDetectorService : IVpnDetectorService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public VpnDetectorService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<bool> IsUsingVpn()
        {
            var forwardedFor = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var ip = forwardedFor?.Split(",").FirstOrDefault().Trim() ??
                     _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ip) || ip == "::1")
                return false;
            string apiKey = _configuration["VpnDetector:ApiKey"];
            using HttpResponseMessage response = await client.GetAsync($"https://vpnapi.io/api/{ip}?key={apiKey}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            VpnResponse vpn = JsonSerializer.Deserialize<VpnResponse>(jsonResponse);
            return vpn.Security.Vpn;
        }
    }
}
