using Microsoft.AspNetCore.Mvc;


namespace SmartLinkClient.Controllers
{
    public class SummaryController : Controller
    {

        [HttpGet]
        public IActionResult MySummaries()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            ViewBag.Id = id;
            return View();
        }

    }
}
