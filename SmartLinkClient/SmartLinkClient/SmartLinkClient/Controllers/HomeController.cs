using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LinkViewModel link)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://smartlinkapi.imaginewebsite.com.tr/api/link");
                var response = await client.PostAsJsonAsync("", new { Url = link.Url, Title = link.Title });
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var resultLink = JsonConvert.DeserializeObject<LinkViewModel>(responseContent);

                    link.Summary = resultLink?.Summary;
                    return Json(new { summary = resultLink?.Summary });
                }
            }
            return NotFound();
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
