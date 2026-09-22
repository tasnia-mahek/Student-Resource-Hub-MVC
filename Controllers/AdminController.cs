using Microsoft.AspNetCore.Authorization;
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

        public AdminController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AcademicCatalog(int? universityId = null, int? departmentId = null, int? semesterId = null)
        {
            return View(await BuildModel(universityId, departmentId, semesterId));
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
            return RedirectToAction(nameof(AcademicCatalog), new { departmentId = form.DepartmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse(CourseFormViewModel form)
        {
            if (ModelState.IsValid && await context.AcademicSemesters.AnyAsync(semester => semester.Id == form.SemesterId && semester.IsActive) && !await context.AcademicCourses.AnyAsync(course => course.AcademicSemesterId == form.SemesterId && course.IsActive && course.CourseCode == form.CourseCode.Trim()))
            {
                context.AcademicCourses.Add(new AcademicCourse { AcademicSemesterId = form.SemesterId, CourseCode = form.CourseCode.Trim(), Name = form.Name.Trim(), IsLab = form.IsLab });
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course added.";
            }
            return RedirectToAction(nameof(AcademicCatalog), new { semesterId = form.SemesterId });
        }

        private async Task<AcademicAdminViewModel> BuildModel(int? universityId, int? departmentId, int? semesterId)
        {
            var universities = await context.Universities
                .Where(university => university.IsActive)
                .Include(university => university.Departments.Where(department => department.IsActive))
                .ThenInclude(department => department.Semesters.Where(semester => semester.IsActive && !semester.AcademicYear.HasValue))
                .ThenInclude(semester => semester.Courses.Where(course => course.IsActive))
                .OrderBy(university => university.Name)
                .ToListAsync();

            return new AcademicAdminViewModel
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
                SelectedSemesterId = semesterId
            };
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
    }
}