using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartLinkClient.Models;

namespace SmartLinkClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://smartlinkapi.imaginewebsite.com.tr/api/");
                var responseTask = client.GetAsync("link");
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                }

            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
