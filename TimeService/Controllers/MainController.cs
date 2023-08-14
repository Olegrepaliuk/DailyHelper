using Microsoft.AspNetCore.Mvc;

namespace TimeService.Controllers
{
    public class MainController : Controller
    {
        [Route("api/main")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}
