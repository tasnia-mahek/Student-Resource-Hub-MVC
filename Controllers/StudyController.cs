using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using student_resource_hub.Data;
using student_resource_hub.Models;
using student_resource_hub.ViewModels;

namespace student_resource_hub.Controllers
{
    [Authorize(Roles = "Student,CR")]
    public class StudyController : Controller
    {
        private readonly ApplicationDbContext context;

        public StudyController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Sessions(string? resourceType = null, int? resourceId = null)
        {
            var userId = CurrentUserId();
            var model = new StudySessionsViewModel
            {
                Sessions = await context.StudySessions
                    .Where(session => session.UserId == userId)
                    .OrderByDescending(session => session.CreatedAt)
                    .Select(session => new StudySessionSummaryViewModel
                    {
                        Id = session.Id,
                        Name = session.Name,
                        Description = session.Description,
                        CreatedAt = session.CreatedAt,
                        FolderCount = session.Folders.Count,
                        ResourceCount = session.Resources.Count
                    }).ToListAsync()
            };
            ViewBag.ResourceType = resourceType;
            ViewBag.ResourceId = resourceId;
            ViewBag.ResourceTitle = await GetResourceTitle(resourceType, resourceId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSession(StudySessionsViewModel model, string? resourceType, int? resourceId)
        {
            if (ModelState.IsValid)
            {
                context.StudySessions.Add(new StudySession { UserId = CurrentUserId(), Name = model.NewSession.Name.Trim(), Description = model.NewSession.Description?.Trim() });
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Sessions), new { resourceType, resourceId });
        }

        [HttpGet]
        public async Task<IActionResult> Session(int id, string? resourceType = null, int? resourceId = null)
        {
            var session = await context.StudySessions
                .Include(item => item.Folders)
                .Include(item => item.Resources)
                .FirstOrDefaultAsync(item => item.Id == id && item.UserId == CurrentUserId());
            if (session == null) return NotFound();
            return View(new StudySessionDetailsViewModel { Session = session, ResourceType = resourceType, ResourceId = resourceId, ResourceTitle = await GetResourceTitle(resourceType, resourceId) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFolder(StudyFolderFormViewModel model)
        {
            if (ModelState.IsValid && await context.StudySessions.AnyAsync(session => session.Id == model.StudySessionId && session.UserId == CurrentUserId()))
            {
                context.StudyFolders.Add(new StudyFolder { StudySessionId = model.StudySessionId, Name = model.Name.Trim() });
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Session), new { id = model.StudySessionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddResource(int sessionId, string resourceType, int resourceId, string? resourceTitle, string? courseCode, int? folderId)
        {
            var session = await context.StudySessions.FirstOrDefaultAsync(item => item.Id == sessionId && item.UserId == CurrentUserId());
            if (session == null) return Forbid();
            var folderIsValid = !folderId.HasValue || await context.StudyFolders.AnyAsync(folder => folder.Id == folderId && folder.StudySessionId == sessionId);
            if (folderIsValid && !await context.StudyResources.AnyAsync(item => item.StudySessionId == sessionId && item.ResourceType == resourceType && item.ResourceId == resourceId))
            {
                context.StudyResources.Add(new StudyResource { StudySessionId = sessionId, StudyFolderId = folderId, ResourceType = resourceType, ResourceId = resourceId, ResourceTitle = resourceTitle ?? "Saved resource", CourseCode = courseCode });
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Session), new { id = sessionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveResource(int id)
        {
            var item = await context.StudyResources.Include(resource => resource.StudySession).FirstOrDefaultAsync(resource => resource.Id == id && resource.StudySession!.UserId == CurrentUserId());
            if (item != null) { context.StudyResources.Remove(item); await context.SaveChangesAsync(); return RedirectToAction(nameof(Session), new { id = item.StudySessionId }); }
            return NotFound();
        }

        private int CurrentUserId() => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

        private async Task<string?> GetResourceTitle(string? type, int? id)
        {
            if (id == null) return null;
            return type?.ToLowerInvariant() switch
            {
                "paper" => await context.PastPapers.Where(item => item.Id == id).Select(item => item.Title).FirstOrDefaultAsync(),
                "note" => await context.Notes.Where(item => item.Id == id).Select(item => item.Title).FirstOrDefaultAsync(),
                "lecture" => await context.Lectures.Where(item => item.Id == id).Select(item => item.Title).FirstOrDefaultAsync(),
                _ => null
            };
        }
    }
}