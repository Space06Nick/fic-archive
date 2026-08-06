using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FicArchive.Web.Data;

namespace FicArchive.Web.Controllers
{
    public class WorksController : Controller
    {
        private readonly AppDbContext _db;

        public WorksController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var works = await _db.Works
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();
            return View(works);
        }

        public async Task<IActionResult> Details(int id)
        {
            var work = await _db.Works
                .Include(w => w.Chapters.OrderBy(c => c.ChapterNumber))
                .FirstOrDefaultAsync(w => w.Id == id);

            if (work == null) return NotFound();

            return View(work);
        }

        public async Task<IActionResult> Read(int id, int chapter = 1)
        {
            var work = await _db.Works
                .Include(w => w.Chapters.OrderBy(c => c.ChapterNumber))
                .FirstOrDefaultAsync(w => w.Id == id);

            if (work == null) return NotFound();

            var current = work.Chapters.FirstOrDefault(c => c.ChapterNumber == chapter)
                          ?? work.Chapters.FirstOrDefault();

            if (current == null) return NotFound();

            ViewData["Chapter"] = current;
            return View(work);
        }
    }
}