using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using student_resource_hub.Data;
using student_resource_hub.Models;
using student_resource_hub.ViewModels;

namespace student_resource_hub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return await Dashboard();
            }
            return View("Landing");
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "Student";
            var userName = User.Identity?.Name ?? "Scholar";
            var userId = GetCurrentUserId();

            var pastPapersCount = await _context.PastPapers.CountAsync(p => p.Status == ResourceStatus.Approved);
            var notesCount = await _context.Notes.CountAsync(n => n.Status == ResourceStatus.Approved);
            var lecturesCount = await _context.Lectures.CountAsync(l => l.Status == ResourceStatus.Approved);

            var activeSessions = await _context.AttendanceSessions
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.SessionDate)
                .ThenByDescending(s => s.StartTime)
                .Take(5)
                .ToListAsync();

            var userAttendedSessions = await _context.AttendanceRecords
                .Where(r => r.UserId == userId)
                .Select(r => r.AttendanceSessionId)
                .ToListAsync();

            var totalSessions = await _context.AttendanceSessions.CountAsync();

            var recentPapers = await _context.PastPapers
                .Where(p => p.Status == ResourceStatus.Approved)
                .Include(p => p.UploadedByUser)
                .OrderByDescending(p => p.CreatedDate)
                .Take(4)
                .ToListAsync();

            var recentNotes = await _context.Notes
                .Where(n => n.Status == ResourceStatus.Approved)
                .Include(n => n.UploadedByUser)
                .OrderByDescending(n => n.CreatedDate)
                .Take(4)
                .ToListAsync();

            var recentLectures = await _context.Lectures
                .Where(l => l.Status == ResourceStatus.Approved)
                .Include(l => l.UploadedByUser)
                .OrderByDescending(l => l.CreatedDate)
                .Take(3)
                .ToListAsync();

            var model = new DashboardViewModel
            {
                UserName = userName,
                UserEmail = userEmail,
                UserRole = userRole,
                TotalPastPapers = pastPapersCount,
                TotalNotes = notesCount,
                TotalLectures = lecturesCount,
                TotalFavorites = 3,
                TotalAvailableSessions = totalSessions,
                TotalAttendedSessions = userAttendedSessions.Count,
                ActiveAttendanceSessions = activeSessions,
                AttendedSessionIds = userAttendedSessions.ToHashSet(),
                RecentPastPapers = recentPapers,
                RecentNotes = recentNotes,
                RecentLectures = recentLectures
            };

            return View("Dashboard", model);
        }

        public IActionResult Landing()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [Authorize]
        public IActionResult Lectures()
        {
            return RedirectToAction("Lectures", "Resource");
        }

        [Authorize]
        public IActionResult PastPapers()
        {
            return RedirectToAction("PastPapers", "Resource");
        }

        [Authorize]
        public IActionResult Notes()
        {
            return RedirectToAction("Notes", "Resource");
        }

        [Authorize]
        public IActionResult Favorites()
        {
            return View();
        }

        [Authorize]
        public IActionResult Notifications()
        {
            return View();
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

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(idClaim, out var id) && id > 0)
            {
                return id;
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null) return user.Id;
            }

            return 0;
        }
    }
}
