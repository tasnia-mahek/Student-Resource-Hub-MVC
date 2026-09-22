using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using student_resource_hub.Data;
using student_resource_hub.Models;
using student_resource_hub.ViewModels;

namespace student_resource_hub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IPasswordHasher<User> passwordHasher;

        public AdminController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
        }

        [HttpGet]
        public async Task<IActionResult> Moderation()
        {
            var requests = await context.ResourceModerationRequests.Include(request => request.RequestedByUser).Where(request => request.Status == "Pending").OrderBy(request => request.CreatedAt).ToListAsync();
            ViewBag.PendingUploads = (await context.PastPapers.Where(item => item.Status == ResourceStatus.Pending).Select(item => new PendingResourceItemViewModel { ResourceType = "paper", ResourceId = item.Id, Title = item.Title, UploadedBy = item.UploadedByUser!.FullName }).ToListAsync())
                .Concat(await context.Notes.Where(item => item.Status == ResourceStatus.Pending).Select(item => new PendingResourceItemViewModel { ResourceType = "note", ResourceId = item.Id, Title = item.Title, UploadedBy = item.UploadedByUser!.FullName }).ToListAsync())
                .Concat(await context.Lectures.Where(item => item.Status == ResourceStatus.Pending).Select(item => new PendingResourceItemViewModel { ResourceType = "lecture", ResourceId = item.Id, Title = item.Title, UploadedBy = item.UploadedByUser!.FullName }).ToListAsync()).ToList();
            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewUpload(int id, string type, bool approve)
        {
            if (type == "paper") { var item = await context.PastPapers.FindAsync(id); if (item != null) item.Status = approve ? ResourceStatus.Approved : ResourceStatus.Rejected; }
            if (type == "note") { var item = await context.Notes.FindAsync(id); if (item != null) item.Status = approve ? ResourceStatus.Approved : ResourceStatus.Rejected; }
            if (type == "lecture") { var item = await context.Lectures.FindAsync(id); if (item != null) item.Status = approve ? ResourceStatus.Approved : ResourceStatus.Rejected; }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Moderation));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewModeration(int id, bool approve)
        {
            var request = await context.ResourceModerationRequests.FindAsync(id);
            if (request == null) return NotFound();
            request.Status = approve ? "Approved" : "Rejected";
            request.ReviewedAt = DateTime.UtcNow;
            request.ReviewedByUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            if (approve && request.Action == "Delete")
            {
                switch (request.ResourceType)
                {
                    case "paper": var paper = await context.PastPapers.FindAsync(request.ResourceId); if (paper != null) context.PastPapers.Remove(paper); break;
                    case "note": var note = await context.Notes.FindAsync(request.ResourceId); if (note != null) context.Notes.Remove(note); break;
                    case "lecture": var lecture = await context.Lectures.FindAsync(request.ResourceId); if (lecture != null) context.Lectures.Remove(lecture); break;
                }
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Moderation));
        }

        [HttpGet]
        public async Task<IActionResult> AcademicCatalog(int? universityId = null, int? departmentId = null, int? semesterId = null, string mode = "resources")
        {
            return View(await BuildModel(universityId, departmentId, semesterId, mode));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(RegisterViewModel form)
        {
            ModelState.Remove(nameof(form.Role));
            ModelState.Remove(nameof(form.UniversityId));
            ModelState.Remove(nameof(form.AcademicDepartmentId));
            ModelState.Remove(nameof(form.AcademicSemesterId));
            if (ModelState.IsValid && !await context.Users.AnyAsync(user => user.Email == form.Email.Trim().ToLowerInvariant()))
            {
                var admin = new User { FullName = form.FullName.Trim(), Email = form.Email.Trim().ToLowerInvariant(), Role = "Admin" };
                admin.PasswordHash = passwordHasher.HashPassword(admin, form.Password);
                context.Users.Add(admin);
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Admin account created.";
            }
            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromoteToCr(int id, int? universityId, int? departmentId, int? semesterId, string mode = "users")
        {
            var user = await context.Users.FindAsync(id);
            if (user != null && user.Role == "Student") { user.Role = "CR"; await context.SaveChangesAsync(); }
            return RedirectToAction(nameof(AcademicCatalog), new { universityId, departmentId, semesterId, mode });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUser(int id, int? universityId, int? departmentId, int? semesterId, string mode = "users")
        {
            var user = await context.Users.FindAsync(id);
            if (user != null && user.Role != "Admin") { context.Users.Remove(user); await context.SaveChangesAsync(); }
            return RedirectToAction(nameof(AcademicCatalog), new { universityId, departmentId, semesterId, mode });
        }

        [HttpGet]
        public async Task<IActionResult> ResourceBrowser(string resourceType = "paper", int? universityId = null, int? departmentId = null, int? semesterId = null)
        {
            var type = resourceType is "note" or "lecture" ? resourceType : "paper";
            var universities = await context.Universities
                .Where(university => university.IsActive)
                .Include(university => university.Departments.Where(department => department.IsActive))
                .ThenInclude(department => department.Semesters.Where(semester => semester.IsActive && !semester.AcademicYear.HasValue))
                .ThenInclude(semester => semester.Courses.Where(course => course.IsActive))
                .OrderBy(university => university.Name)
                .ToListAsync();
            var model = new AdminResourceBrowseViewModel
            {
                ResourceType = type,
                UniversityId = universityId,
                DepartmentId = departmentId,
                SemesterId = semesterId,
                Universities = universities.Select(MapResourceUniversity).ToList()
            };
            model.University = model.Universities.FirstOrDefault(item => item.Id == universityId);
            model.Department = model.University?.Departments.FirstOrDefault(item => item.Id == departmentId);
            model.Semester = model.Department?.Semesters.FirstOrDefault(item => item.Id == semesterId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchUniversities(string? term, string resourceType = "paper")
        {
            var type = resourceType is "note" or "lecture" ? resourceType : "paper";
            var results = await context.Universities
                .Where(university => university.IsActive && (string.IsNullOrWhiteSpace(term) || university.Name.Contains(term.Trim())))
                .OrderBy(university => university.Name)
                .Take(8)
                .Select(university => new { university.Name, url = Url.Action(nameof(ResourceBrowser), new { resourceType = type, universityId = university.Id }) })
                .ToListAsync();
            return Json(results);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUniversity(UniversityFormViewModel form)
        {
            if (ModelState.IsValid && !await context.Universities.AnyAsync(university => university.IsActive && university.Name == form.Name.Trim()))
            {
                context.Universities.Add(new University { Name = form.Name.Trim() });
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "University added.";
            }
            return RedirectToAction(nameof(AcademicCatalog));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment(DepartmentFormViewModel form)
        {
            if (ModelState.IsValid && await context.Universities.AnyAsync(university => university.Id == form.UniversityId && university.IsActive) && !await context.AcademicDepartments.AnyAsync(department => department.UniversityId == form.UniversityId && department.IsActive && department.Name == form.Name.Trim()))
            {
                context.AcademicDepartments.Add(new AcademicDepartment
                {
                    UniversityId = form.UniversityId,
                    Name = form.Name.Trim(),
                    ImageUrl = string.IsNullOrWhiteSpace(form.ImageUrl) ? GetDepartmentImage(context.AcademicDepartments.Count()) : form.ImageUrl.Trim()
                });
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Department added.";
            }
            return RedirectToAction(nameof(AcademicCatalog), new { universityId = form.UniversityId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSemester(SemesterFormViewModel form)
        {
            var loadedDepartment = await context.AcademicDepartments.AsNoTracking().FirstOrDefaultAsync(item => item.Id == form.DepartmentId);
            if (ModelState.IsValid && await context.AcademicDepartments.AnyAsync(department => department.Id == form.DepartmentId && department.IsActive) && !await context.AcademicSemesters.AnyAsync(semester => semester.DepartmentId == form.DepartmentId && semester.IsActive && semester.SortOrder == form.SemesterNumber))
            {
                var semesterNumber = form.SemesterNumber;
                context.AcademicSemesters.Add(new AcademicSemester
                {
                    DepartmentId = form.DepartmentId,
                    Name = $"Semester {semesterNumber}",
                    SortOrder = semesterNumber
                });
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Semester added.";
            }
            return RedirectToAction(nameof(AcademicCatalog), new { universityId = loadedDepartment?.UniversityId, departmentId = form.DepartmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse(CourseFormViewModel form)
        {
            var loadedSemester = await context.AcademicSemesters.AsNoTracking().FirstOrDefaultAsync(item => item.Id == form.SemesterId);
            var departmentId = loadedSemester?.DepartmentId;
            var universityId = departmentId.HasValue ? await context.AcademicDepartments.Where(item => item.Id == departmentId.Value).Select(item => (int?)item.UniversityId).FirstOrDefaultAsync() : null;
            if (ModelState.IsValid && await context.AcademicSemesters.AnyAsync(semester => semester.Id == form.SemesterId && semester.IsActive) && !await context.AcademicCourses.AnyAsync(course => course.AcademicSemesterId == form.SemesterId && course.IsActive && course.CourseCode == form.CourseCode.Trim()))
            {
                context.AcademicCourses.Add(new AcademicCourse { AcademicSemesterId = form.SemesterId, CourseCode = form.CourseCode.Trim(), Name = form.Name.Trim(), IsLab = form.IsLab });
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course added.";
            }
            return RedirectToAction(nameof(AcademicCatalog), new { universityId, departmentId, semesterId = form.SemesterId });
        }

        private async Task<AcademicAdminViewModel> BuildModel(int? universityId, int? departmentId, int? semesterId, string mode)
        {
            var universities = await context.Universities
                .Where(university => university.IsActive)
                .Include(university => university.Departments.Where(department => department.IsActive))
                .ThenInclude(department => department.Semesters.Where(semester => semester.IsActive && !semester.AcademicYear.HasValue))
                .ThenInclude(semester => semester.Courses.Where(course => course.IsActive))
                .OrderBy(university => university.Name)
                .ToListAsync();

            var model = new AcademicAdminViewModel
            {
                Universities = universities.Select(university => new UniversityAdminItemViewModel
                {
                    Id = university.Id,
                    Name = university.Name,
                    Departments = university.Departments.OrderBy(department => department.Name).Select(department => new DepartmentAdminItemViewModel
                    {
                        Id = department.Id,
                        Name = department.Name,
                        ImageUrl = department.ImageUrl,
                        Semesters = department.Semesters.OrderBy(semester => semester.SortOrder).Select(semester => new SemesterAdminItemViewModel { Id = semester.Id, Name = semester.Name, SortOrder = semester.SortOrder, Courses = semester.Courses.OrderBy(course => course.CourseCode).Select(course => new CourseAdminItemViewModel { Id = course.Id, CourseCode = course.CourseCode, Name = course.Name, IsLab = course.IsLab }).ToList() }).ToList()
                    }).ToList()
                }).ToList(),
                SelectedUniversityId = universityId,
                SelectedDepartmentId = departmentId,
                SelectedSemesterId = semesterId,
                Mode = mode
            };
            if (mode == "users" && semesterId.HasValue)
            {
                model.Users = await context.Users
                    .Where(user => user.AcademicSemesterId == semesterId.Value)
                    .OrderBy(user => user.FullName)
                    .Select(user => new AdminUserItemViewModel { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = user.Role, SemesterId = user.AcademicSemesterId })
                    .ToListAsync();
            }
            return model;
        }

        private static string GetDepartmentImage(int index)
        {
            var images = new[]
            {
                "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=1200&q=80",
                "https://images.unsplash.com/photo-1581092160607-ee22621dd758?auto=format&fit=crop&w=1200&q=80",
                "https://images.unsplash.com/photo-1517245386807-bb43f82c33c4?auto=format&fit=crop&w=1200&q=80"
            };
            return images[index % images.Length];
        }

        private static AdminResourceUniversityViewModel MapResourceUniversity(University university) => new()
        {
            Id = university.Id,
            Name = university.Name,
            Departments = university.Departments.OrderBy(department => department.Name).Select(department => new AdminResourceDepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
                ImageUrl = department.ImageUrl,
                Semesters = department.Semesters.OrderBy(semester => semester.SortOrder).Select(semester => new AdminResourceSemesterViewModel
                {
                    Id = semester.Id,
                    Name = semester.Name,
                    SortOrder = semester.SortOrder,
                    Courses = semester.Courses.OrderBy(course => course.CourseCode).Select(course => new AdminResourceCourseViewModel { CourseCode = course.CourseCode, Name = course.Name }).ToList()
                }).ToList()
            }).ToList()
        };
    }
}