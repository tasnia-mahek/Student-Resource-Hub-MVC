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
        public async Task<IActionResult> AcademicCatalog()
        {
            return View(await BuildModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUniversity(UniversityFormViewModel form)
        {
            if (ModelState.IsValid)
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
            if (ModelState.IsValid && await context.Universities.AnyAsync(university => university.Id == form.UniversityId && university.IsActive))
            {
                context.AcademicDepartments.Add(new AcademicDepartment
                {
                    UniversityId = form.UniversityId,
                    Name = form.Name.Trim(),
                    ImageUrl = string.IsNullOrWhiteSpace(form.ImageUrl) ? null : form.ImageUrl.Trim()
                });
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Department added.";
            }
            return RedirectToAction(nameof(AcademicCatalog));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSemester(SemesterFormViewModel form)
        {
            if (ModelState.IsValid && await context.AcademicDepartments.AnyAsync(department => department.Id == form.DepartmentId && department.IsActive))
            {
                context.AcademicSemesters.Add(new AcademicSemester
                {
                    DepartmentId = form.DepartmentId,
                    Name = form.Name.Trim(),
                    SortOrder = form.SortOrder
                });
                await context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Semester added.";
            }
            return RedirectToAction(nameof(AcademicCatalog));
        }

        private async Task<AcademicAdminViewModel> BuildModel()
        {
            var universities = await context.Universities
                .Where(university => university.IsActive)
                .Include(university => university.Departments.Where(department => department.IsActive))
                .ThenInclude(department => department.Semesters.Where(semester => semester.IsActive))
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
                        Semesters = department.Semesters.OrderBy(semester => semester.SortOrder).Select(semester => new SemesterAdminItemViewModel { Id = semester.Id, Name = semester.Name }).ToList()
                    }).ToList()
                }).ToList()
            };
        }
    }
}