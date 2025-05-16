using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Msar_Gded;
using Msar_Gded.Models;

namespace IDS.Controllers
{
    public class AdminController : Controller
    {

        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {


            ViewData["Statuses"] = new SelectList(_context.Statuses, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string load)
        {
            ViewBag.LoadPartial = load;
            ViewData["Statuses"] = new SelectList(_context.Statuses, "Id", "Name");


            return View();
        }

    }
}
