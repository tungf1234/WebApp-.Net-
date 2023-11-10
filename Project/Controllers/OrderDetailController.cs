using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly NorthWindContext _context;
        public OrderDetailController(NorthWindContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
