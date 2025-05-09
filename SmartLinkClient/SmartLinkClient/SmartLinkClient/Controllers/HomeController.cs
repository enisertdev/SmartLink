using System.Diagnostics;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartLinkClient.Interfaces;
using SmartLinkClient.Models;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace SmartLinkClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVpnDetectorService _vpnDetectorService;

        public HomeController(ILogger<HomeController> logger, IVpnDetectorService vpnDetectorService)
        {
            _logger = logger;
            _vpnDetectorService = vpnDetectorService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           // if (await _vpnDetectorService.IsUsingVpn())
             //   return RedirectToAction("ErrorVpn", "Error");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

      
   

    }
}