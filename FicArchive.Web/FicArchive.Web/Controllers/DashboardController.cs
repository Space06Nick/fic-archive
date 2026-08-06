using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FicArchive.Web.Data;
using FicArchive.Web.Models;

namespace FicArchive.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Works()
        {
            var username = User.Identity!.Name!;
            var works = await _db.Works
                .Where(w => w.AuthorName == username)
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();
            return View(works);
        }

        // ---------- Post New Work ----------
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string title, string summary, string rating, string fandoms,
            string category, string relationships, string characters, string additionalTags,
            string chapterTitle, string content)
        {
            if (string.IsNullOrWhiteSpace(title)) ModelState.AddModelError("", "Work Title is required.");
            if (string.IsNullOrWhiteSpace(fandoms)) ModelState.AddModelError("", "Fandoms are required.");
            if (string.IsNullOrWhiteSpace(content)) ModelState.AddModelError("", "Work Text is required.");

            if (!ModelState.IsValid) return View();

            var work = new Work
            {
                Title = title.Trim(),
                Summary = summary?.Trim(),
                Rating = string.IsNullOrWhiteSpace(rating) ? "Not Rated" : rating,
                Fandoms = fandoms.Trim(),
                Category = category?.Trim(),
                Relationships = relationships?.Trim(),
                Characters = characters?.Trim(),
                AdditionalTags = additionalTags?.Trim(),
                AuthorName = User.Identity!.Name!,
                CreatedAt = DateTime.UtcNow
            };

            _db.Works.Add(work);
            await _db.SaveChangesAsync();

            var chapter = new Chapter
            {
                WorkId = work.Id,
                ChapterNumber = 1,
                Title = string.IsNullOrWhiteSpace(chapterTitle) ? "Chapter 1" : chapterTitle.Trim(),
                Content = content
            };
            _db.Chapters.Add(chapter);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Works", new { id = work.Id });
        }

        // ---------- Add Chapter ----------
        public async Task<IActionResult> AddChapter(int workId)
        {
            var work = await _db.Works.FindAsync(workId);
            if (work == null || work.AuthorName != User.Identity!.Name)
            {
                return NotFound();
            }
            return View(work);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChapter(int workId, string title, string content)
        {
            var work = await _db.Works.FindAsync(workId);
            if (work == null || work.AuthorName != User.Identity!.Name)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError("", "Chapter text is required.");
                return View(work);
            }

            var lastNumber = await _db.Chapters
                .Where(c => c.WorkId == workId)
                .MaxAsync(c => (int?)c.ChapterNumber) ?? 0;

            var chapter = new Chapter
            {
                WorkId = workId,
                ChapterNumber = lastNumber + 1,
                Title = string.IsNullOrWhiteSpace(title) ? $"Chapter {lastNumber + 1}" : title.Trim(),
                Content = content
            };

            _db.Chapters.Add(chapter);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Works", new { id = workId });
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult Preferences()
        {
            return View();
        }
    }
}