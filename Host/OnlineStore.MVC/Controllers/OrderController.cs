using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.MVC.Controllers
{
    public class OrderController : Controller
    {
        public OrderController()
        {
            
        }
        public IActionResult AddOrder()
        {
            
            return View("OrderView");
        }
    }
}
