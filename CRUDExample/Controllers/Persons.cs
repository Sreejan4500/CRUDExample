using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
    public class Persons : Controller
    {
        [Route("persons/index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
