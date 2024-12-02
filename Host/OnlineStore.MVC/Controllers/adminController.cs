using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.MVC.Controllers
{
    public class adminController : Controller
    {
        [HttpGet]
        public IActionResult adminPanel()
        {
            return View("adminPanelView");
        }
    }
}
