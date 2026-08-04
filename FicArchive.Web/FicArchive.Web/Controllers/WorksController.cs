using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FicArchive.Web.Data;
using FicArchive.Web.Models;

namespace FicArchive.Web.Controllers
{
    public class WorksController : Controller
    {
        private readonly AppDbContext _db;

        public WorksController(AppDbContext db)
        {
            _db = db;
        }

        // Страница истории со списком глав
        public IActionResult Details(int id)
        {
            var work = _db.Works
                .Include(w => w.Chapters)
                .FirstOrDefault(w => w.Id == id);

            if (work == null)
            {
                return NotFound();
            }

            return View(work);
        }

        // Страница чтения главы
        public IActionResult Read(int id)
        {
            var chapter = _db.Chapters
                .Include(c => c.Work)
                .FirstOrDefault(c => c.Id == id);

            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }
    }
}