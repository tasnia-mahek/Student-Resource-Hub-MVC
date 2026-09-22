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
        public async Task<IActionResult> Sessions(string? resourceType = null, int? resourceId = null, string? courseCode = null, int? studySessionId = null, int? studyFolderId = null, string? returnUrl = null)
        {
            var userId = CurrentUserId();
            if (studySessionId.HasValue && await context.StudySessions.AnyAsync(session => session.Id == studySessionId.Value && session.UserId == userId))
            {
                return RedirectToAction(nameof(Session), new { id = studySessionId.Value, resourceType, resourceId, courseCode, studyFolderId, returnUrl });
            }
            await CleanStudyData(userId);
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
            ViewBag.CourseCode = courseCode;
            ViewBag.StudySessionId = studySessionId;
            ViewBag.StudyFolderId = studyFolderId;
            ViewBag.ResourceTitle = await GetResourceTitle(resourceType, resourceId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSession(StudySessionsViewModel model, string? resourceType, int? resourceId, string? courseCode)
        {
            if (ModelState.IsValid)
            {
                var session = new StudySession { UserId = CurrentUserId(), Name = model.NewSession.Name.Trim(), Description = model.NewSession.Description?.Trim() };
                context.StudySessions.Add(session);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Session), new { id = session.Id, resourceType, resourceId, courseCode });
            }
            return RedirectToAction(nameof(Sessions), new { resourceType, resourceId, courseCode });
        }

        [HttpGet]
        public async Task<IActionResult> Session(int id, string? resourceType = null, int? resourceId = null, string? courseCode = null, int? studyFolderId = null, string? returnUrl = null)
        {
            await CleanStudyData(CurrentUserId(), id);
            var session = await context.StudySessions
                .Include(item => item.Folders)
                .Include(item => item.Resources)
                .FirstOrDefaultAsync(item => item.Id == id && item.UserId == CurrentUserId());
            if (session == null) return NotFound();
            var currentFolder = studyFolderId.HasValue
                ? session.Folders.FirstOrDefault(folder => folder.Id == studyFolderId.Value)
                : null;
            if (studyFolderId.HasValue && currentFolder == null) return NotFound();
            return View(new StudySessionDetailsViewModel { Session = session, ResourceType = resourceType, ResourceId = resourceId, ResourceTitle = await GetResourceTitle(resourceType, resourceId), CourseCode = courseCode, SelectedFolderId = studyFolderId, CurrentFolderId = studyFolderId, CurrentFolderName = currentFolder?.Name, ReturnUrl = IsLocalUrl(returnUrl) ? returnUrl : null });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFolder(StudyFolderFormViewModel model, string? resourceType, int? resourceId, string? courseCode, string? returnUrl = null)
        {
            var sessionId = model.StudySessionId;
            var parentIsValid = !model.ParentFolderId.HasValue || await context.StudyFolders.AnyAsync(folder => folder.Id == model.ParentFolderId && folder.StudySessionId == sessionId);
            if (ModelState.IsValid && parentIsValid && await context.StudySessions.AnyAsync(session => session.Id == sessionId && session.UserId == CurrentUserId()))
            {
                context.StudyFolders.Add(new StudyFolder { StudySessionId = sessionId, ParentFolderId = model.ParentFolderId, Name = model.Name.Trim() });
                await context.SaveChangesAsync();
            }
            return sessionId > 0
                ? RedirectToAction(nameof(Session), new { id = sessionId, resourceType, resourceId, courseCode, studyFolderId = model.ParentFolderId, returnUrl })
                : RedirectToAction(nameof(Sessions), new { resourceType, resourceId, courseCode });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddResource(int sessionId, string resourceType, int resourceId, string? resourceTitle, string? courseCode, int? folderId, string? returnUrl = null)
        {
            var session = await context.StudySessions.FirstOrDefaultAsync(item => item.Id == sessionId && item.UserId == CurrentUserId());
            if (session == null) return Forbid();
            var folderIsValid = !folderId.HasValue || await context.StudyFolders.AnyAsync(folder => folder.Id == folderId && folder.StudySessionId == sessionId);
            var normalizedType = resourceType?.Trim().ToLowerInvariant();
            var resource = await GetApprovedResource(resourceType, resourceId);
            if (folderIsValid && resource != null && !await context.StudyResources.AnyAsync(item => item.StudySessionId == sessionId && item.ResourceType == normalizedType && item.ResourceId == resourceId))
            {
                context.StudyResources.Add(new StudyResource
                {
                    StudySessionId = sessionId,
                    StudyFolderId = folderId,
                    ResourceType = normalizedType!,
                    ResourceId = resourceId,
                    ResourceTitle = resource.Value.Title,
                    CourseCode = resource.Value.CourseCode
                });
                await context.SaveChangesAsync();
            }
            if (IsLocalUrl(returnUrl)) return LocalRedirect(returnUrl!);
            return RedirectToAction(nameof(Session), new { id = sessionId, resourceType = normalizedType, resourceId, courseCode = resource?.CourseCode ?? courseCode, studyFolderId = folderId });
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

        private bool IsLocalUrl(string? url) => !string.IsNullOrWhiteSpace(url) && Url.IsLocalUrl(url);

        private async Task CleanStudyData(int userId, int? sessionId = null)
        {
            var sessions = await context.StudySessions
                .Where(session => session.UserId == userId && (!sessionId.HasValue || session.Id == sessionId.Value))
                .Select(session => new { session.Id, FolderIds = session.Folders.Select(folder => folder.Id) })
                .ToListAsync();
            var sessionIds = sessions.Select(session => session.Id).ToList();
            if (sessionIds.Count == 0) return;

            var folderIds = sessions.SelectMany(session => session.FolderIds).ToHashSet();
            var saved = await context.StudyResources.Where(resource => sessionIds.Contains(resource.StudySessionId)).ToListAsync();
            var valid = new List<StudyResource>();
            foreach (var item in saved)
            {
                var approved = await GetApprovedResource(item.ResourceType, item.ResourceId);
                if (approved == null || item.StudyFolderId.HasValue && !folderIds.Contains(item.StudyFolderId.Value))
                {
                    context.StudyResources.Remove(item);
                    continue;
                }
                item.ResourceTitle = approved.Value.Title;
                item.CourseCode = approved.Value.CourseCode;
                valid.Add(item);
            }
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }

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

        private async Task<(string Title, string CourseCode)?> GetApprovedResource(string? type, int id)
        {
            switch (type?.Trim().ToLowerInvariant())
            {
                case "paper":
                    var paper = await context.PastPapers.Where(item => item.Id == id && item.Status == ResourceStatus.Approved).Select(item => new { item.Title, item.CourseCode }).FirstOrDefaultAsync();
                    return paper == null ? null : (paper.Title, paper.CourseCode);
                case "note":
                    var note = await context.Notes.Where(item => item.Id == id && item.Status == ResourceStatus.Approved).Select(item => new { item.Title, item.CourseCode }).FirstOrDefaultAsync();
                    return note == null ? null : (note.Title, note.CourseCode);
                case "lecture":
                    var lecture = await context.Lectures.Where(item => item.Id == id && item.Status == ResourceStatus.Approved).Select(item => new { item.Title, item.CourseCode }).FirstOrDefaultAsync();
                    return lecture == null ? null : (lecture.Title, lecture.CourseCode);
                default:
                    return null;
            }
        }
    }
}