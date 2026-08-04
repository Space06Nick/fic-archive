using FicArchive.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FicArchive.Web.Data;
using FicArchive.Web.Models;

namespace FicArchive.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var works = _db.Works.ToList();
            return View(works);
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
