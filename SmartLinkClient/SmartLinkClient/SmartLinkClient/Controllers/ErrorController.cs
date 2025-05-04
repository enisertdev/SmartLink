using Microsoft.AspNetCore.Mvc;


namespace SmartLinkClient.Controllers
{
    public class ErrorController : Controller
    {

        [HttpGet]
        public IActionResult Index(int statusCode)
        {
            if(statusCode == 401)
            {
                return View("Unathorized");
            }
            return NotFound();
        }

    }
}
