using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using student_resource_hub.Data;
using student_resource_hub.Models;
using student_resource_hub.ViewModels;

namespace student_resource_hub.Controllers
{
    [Authorize]
    public class StarterKitController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StarterKitController> _logger;

        public StarterKitController(
            ApplicationDbContext context,
            ILogger<StarterKitController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /StarterKit
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string? department = null, [FromQuery] string? semester = null)
        {
            var viewModel = new StarterKitViewModel
            {
                SelectedDepartment = department,
                SelectedSemester = semester
            };

            if (!string.IsNullOrWhiteSpace(department) && !string.IsNullOrWhiteSpace(semester))
            {
                bool isDeptValid = Enum.TryParse<Department>(department, true, out var deptEnum);
                bool isSemValid = Enum.TryParse<Semester>(semester, true, out var semEnum);

                if (isDeptValid && isSemValid)
                {
                    viewModel.IsGenerated = true;
                    try
                    {
                        // 1. Fetch all approved past papers for selected department and semester
                        var papers = await _context.PastPapers
                            .Where(p => p.Status == ResourceStatus.Approved && p.Department == deptEnum && p.Semester == semEnum)
                            .Include(p => p.UploadedByUser)
                            .OrderByDescending(p => p.CreatedDate)
                            .ToListAsync();

                        // 2. Fetch all approved notes for selected department and semester
                        var notes = await _context.Notes
                            .Where(n => n.Status == ResourceStatus.Approved && n.Department == deptEnum && n.Semester == semEnum)
                            .Include(n => n.UploadedByUser)
                            .OrderByDescending(n => n.CreatedDate)
                            .ToListAsync();

                        // 3. Fetch all approved video lectures for selected department and semester
                        var lectures = await _context.Lectures
                            .Where(l => l.Status == ResourceStatus.Approved && l.Department == deptEnum && l.Semester == semEnum)
                            .Include(l => l.UploadedByUser)
                            .OrderByDescending(l => l.CreatedDate)
                            .ToListAsync();

                        // 4. Group all existing materials by Subject (CourseCode & SubjectName)
                        var subjectMap = new Dictionary<string, StarterKitSubjectGroupViewModel>(StringComparer.OrdinalIgnoreCase);

                        foreach (var paper in papers)
                        {
                            var key = NormalizeKey(paper.CourseCode, paper.SubjectName);
                            if (!subjectMap.TryGetValue(key, out var group))
                            {
                                group = new StarterKitSubjectGroupViewModel
                                {
                                    CourseCode = CleanCourseCode(paper.CourseCode),
                                    SubjectName = CleanSubjectName(paper.SubjectName)
                                };
                                subjectMap[key] = group;
                            }
                            group.PastPapers.Add(paper);
                        }

                        foreach (var note in notes)
                        {
                            var key = NormalizeKey(note.CourseCode, note.SubjectName);
                            if (!subjectMap.TryGetValue(key, out var group))
                            {
                                group = new StarterKitSubjectGroupViewModel
                                {
                                    CourseCode = CleanCourseCode(note.CourseCode),
                                    SubjectName = CleanSubjectName(note.SubjectName)
                                };
                                subjectMap[key] = group;
                            }
                            group.Notes.Add(note);
                        }

                        foreach (var lecture in lectures)
                        {
                            var key = NormalizeKey(lecture.CourseCode, lecture.SubjectName);
                            if (!subjectMap.TryGetValue(key, out var group))
                            {
                                group = new StarterKitSubjectGroupViewModel
                                {
                                    CourseCode = CleanCourseCode(lecture.CourseCode),
                                    SubjectName = CleanSubjectName(lecture.SubjectName)
                                };
                                subjectMap[key] = group;
                            }
                            group.Lectures.Add(lecture);
                        }

                        viewModel.SubjectGroups = subjectMap.Values
                            .OrderBy(s => s.CourseCode)
                            .ThenBy(s => s.SubjectName)
                            .ToList();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error loading starter kit for department {Department}, semester {Semester}", department, semester);
                        ViewBag.ErrorMessage = "An error occurred while loading your Starter Kit. Please try again.";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Please select a valid department and semester.";
                }
            }

            return View(viewModel);
        }

        private static string NormalizeKey(string? courseCode, string? subjectName)
        {
            var code = courseCode?.Replace(" ", "").Trim().ToUpperInvariant() ?? string.Empty;
            if (!string.IsNullOrEmpty(code))
            {
                return code;
            }

            return subjectName?.Replace(" ", "").Trim().ToUpperInvariant() ?? "GENERAL";
        }

        private static string CleanCourseCode(string? code)
        {
            return string.IsNullOrWhiteSpace(code) ? "GEN100" : code.Trim().ToUpperInvariant();
        }

        private static string CleanSubjectName(string? name)
        {
            return string.IsNullOrWhiteSpace(name) ? "General Studies" : name.Trim();
        }
    }
}
