using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using student_resource_hub.Data;
using student_resource_hub.Models;
using student_resource_hub.ViewModels;

namespace student_resource_hub.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(ApplicationDbContext context, ILogger<AttendanceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Attendance
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "Student";
            var userName = User.Identity?.Name ?? "Scholar";

            var isAdmin = userRole == "Admin";
            var isCR = userRole == "CR";
            var isStudent = userRole == "Student";

            var model = new AttendanceDashboardViewModel
            {
                IsAdmin = isAdmin,
                IsCR = isCR,
                IsStudent = isStudent,
                CurrentUserRole = userRole,
                CurrentUserName = userName
            };

            if (isAdmin)
            {
                model.AllSessions = await _context.AttendanceSessions
                    .Include(s => s.AttendanceRecords)
                    .Include(s => s.CreatedByUser)
                    .OrderByDescending(s => s.SessionDate)
                    .ThenByDescending(s => s.StartTime)
                    .ToListAsync();

                model.ActiveSessions = model.AllSessions.Where(s => s.IsActive).ToList();
                model.TotalAvailableSessions = model.AllSessions.Count;
                model.TotalAttendedSessions = model.AllSessions.Count(s => s.AttendanceRecords.Any());
            }
            else
            {
                model.ActiveSessions = await _context.AttendanceSessions
                    .Where(s => s.IsActive)
                    .OrderByDescending(s => s.SessionDate)
                    .ThenByDescending(s => s.StartTime)
                    .ToListAsync();

                model.MyAttendanceRecords = await _context.AttendanceRecords
                    .Where(r => r.UserId == userId)
                    .Include(r => r.AttendanceSession)
                    .OrderByDescending(r => r.SubmittedAt)
                    .ToListAsync();

                model.AttendedSessionIds = model.MyAttendanceRecords
                    .Select(r => r.AttendanceSessionId)
                    .ToHashSet();

                var totalSessions = await _context.AttendanceSessions.CountAsync();
                model.TotalAvailableSessions = totalSessions;
                model.TotalAttendedSessions = model.MyAttendanceRecords.Count;

                if (isCR)
                {
                    // CR can also see all sessions to check attendance roster
                    model.AllSessions = await _context.AttendanceSessions
                        .Include(s => s.AttendanceRecords)
                        .OrderByDescending(s => s.SessionDate)
                        .ToListAsync();
                }
            }

            return View(model);
        }

        // POST: /Attendance/CreateSession (Admin Only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSession(CreateAttendanceSessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields properly.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.FindFirst(ClaimTypes.Email)!.Value);
                    if (user != null) userId = user.Id;
                }

                var session = new AttendanceSession
                {
                    SessionTitle = model.SessionTitle.Trim(),
                    CourseCode = model.CourseCode.Trim().ToUpperInvariant(),
                    Department = model.Department.Trim(),
                    SessionDate = model.SessionDate.Date,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    Passcode = model.Passcode.Trim(),
                    IsActive = true,
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.AttendanceSessions.Add(session);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Attendance session for {session.CourseCode} created successfully! Passcode: {session.Passcode}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating attendance session");
                TempData["ErrorMessage"] = "Failed to create attendance session. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Attendance/ToggleSession/5 (Admin Only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSession(int id)
        {
            var session = await _context.AttendanceSessions.FindAsync(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            session.IsActive = !session.IsActive;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = session.IsActive 
                ? $"Session {session.CourseCode} is now OPEN for attendance check-ins." 
                : $"Session {session.CourseCode} is now CLOSED.";

            return RedirectToAction(nameof(Index));
        }

        // POST: /Attendance/DeleteSession/5 (Admin Only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var session = await _context.AttendanceSessions.FindAsync(id);
            if (session != null)
            {
                _context.AttendanceSessions.Remove(session);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Attendance session deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Attendance/SubmitAttendance (Student & CR)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAttendance(CheckInAttendanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please provide both the session and passcode.";
                return RedirectToAction(nameof(Index));
            }

            var userId = GetCurrentUserId();
            var userName = User.Identity?.Name ?? "Student";
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "Student";

            if (userId == 0)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                if (user != null) userId = user.Id;
            }

            var session = await _context.AttendanceSessions.FirstOrDefaultAsync(s => s.Id == model.SessionId);
            if (session == null)
            {
                TempData["ErrorMessage"] = "The requested attendance session does not exist.";
                return RedirectToAction(nameof(Index));
            }

            if (!session.IsActive)
            {
                TempData["ErrorMessage"] = "This attendance session is currently closed.";
                return RedirectToAction(nameof(Index));
            }

            if (!string.Equals(session.Passcode.Trim(), model.Passcode.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Invalid session passcode. Please ask your instructor or CR.";
                return RedirectToAction(nameof(Index));
            }

            var alreadyCheckedIn = await _context.AttendanceRecords
                .AnyAsync(r => r.AttendanceSessionId == session.Id && r.UserId == userId);

            if (alreadyCheckedIn)
            {
                TempData["ErrorMessage"] = "You have already submitted attendance for this session.";
                return RedirectToAction(nameof(Index));
            }

            var record = new AttendanceRecord
            {
                AttendanceSessionId = session.Id,
                UserId = userId,
                StudentName = userName,
                StudentEmail = userEmail,
                Role = userRole,
                SubmittedAt = DateTime.UtcNow,
                Status = "Present"
            };

            _context.AttendanceRecords.Add(record);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Attendance marked as PRESENT for {session.CourseCode} ({session.SessionTitle})!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Attendance/Roster/5 (Admin & CR)
        [HttpGet]
        [Authorize(Roles = "Admin,CR")]
        public async Task<IActionResult> Roster(int id)
        {
            var session = await _context.AttendanceSessions
                .Include(s => s.CreatedByUser)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound("Attendance session not found.");
            }

            var attendees = await _context.AttendanceRecords
                .Where(r => r.AttendanceSessionId == id)
                .OrderBy(r => r.SubmittedAt)
                .ToListAsync();

            var model = new SessionRosterViewModel
            {
                Session = session,
                Attendees = attendees,
                CanManage = User.IsInRole("Admin")
            };

            return View(model);
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
