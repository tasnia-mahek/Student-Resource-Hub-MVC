using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using student_resource_hub.Data;
using student_resource_hub.Models;
using student_resource_hub.Services;
using student_resource_hub.ViewModels;

namespace student_resource_hub.Controllers
{
    [Authorize]
    public class ResourceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly ILogger<ResourceController> _logger;

        public ResourceController(
            ApplicationDbContext context,
            IFileService fileService,
            ILogger<ResourceController> logger)
        {
            _context = context;
            _fileService = fileService;
            _logger = logger;
        }

        // GET: Resource/PastPapers
        [HttpGet]
        public async Task<IActionResult> PastPapers(
            [FromQuery] string? department = null,
            [FromQuery] int? year = null,
            [FromQuery] string? semester = null,
            [FromQuery] string? sortBy = "newest",
            [FromQuery] string? search = null,
            [FromQuery] int? academicDepartmentId = null,
            [FromQuery] int? academicSemesterId = null,
            [FromQuery] int? academicTermId = null,
            [FromQuery] string? courseCode = null)
        {
            try
            {
                ViewBag.StudySessionId = Request.Query["studySessionId"].FirstOrDefault();
                ViewBag.StudyFolderId = Request.Query["studyFolderId"].FirstOrDefault();
                if (!academicDepartmentId.HasValue && string.IsNullOrWhiteSpace(department))
                {
                    return View(await BuildPastPaperDepartments());
                }

                if (academicDepartmentId.HasValue && !academicSemesterId.HasValue)
                {
                    return View(await BuildPastPaperSemesters(academicDepartmentId.Value, await GetCurrentTermId(academicDepartmentId.Value)));
                }

                if (academicDepartmentId.HasValue && academicSemesterId.HasValue)
                {
                    var currentTermId = academicTermId.HasValue ? (academicTermId.Value > 0 ? academicTermId : null) : await GetCurrentTermId(academicDepartmentId.Value);
                    if (string.IsNullOrWhiteSpace(courseCode))
                    {
                        return View(new ResourceListViewModel<PastPaper>
                        {
                            SelectedAcademicDepartmentId = academicDepartmentId,
                            SelectedAcademicSemesterId = academicSemesterId,
                            SelectedAcademicTermId = academicTermId ?? currentTermId,
                            SemesterGroups = (await BuildPastPaperSemesters(academicDepartmentId.Value, currentTermId)).SemesterGroups,
                            CourseCatalogGroups = await BuildPastPaperCourses(academicSemesterId.Value, currentTermId),
                            ResourceTerms = await BuildResourceTerms(academicDepartmentId.Value)
                        });
                    }
                    var dynamicQuery = _context.PastPapers
                        .Where(p => p.Status == ResourceStatus.Approved && p.AcademicDepartmentId == academicDepartmentId && p.AcademicSemesterId == currentTermId && p.CourseCode == courseCode);
                    dynamicQuery = ApplyPastPaperSearch(dynamicQuery, search);
                    dynamicQuery = sortBy switch
                    {
                        "oldest" => dynamicQuery.OrderBy(p => p.CreatedDate),
                        "mostDownloaded" => dynamicQuery.OrderByDescending(p => p.DownloadCount),
                        _ => dynamicQuery.OrderByDescending(p => p.CreatedDate)
                    };
                    var papers = await dynamicQuery.Include(p => p.UploadedByUser).ToListAsync();
                    return View(new ResourceListViewModel<PastPaper> { Resources = papers, CourseGroups = BuildCourseGroups(papers, paper => paper.CourseCode, paper => paper.SubjectName), CourseCatalogGroups = await BuildPastPaperCourses(academicSemesterId.Value, currentTermId), SemesterGroups = (await BuildPastPaperSemesters(academicDepartmentId.Value, currentTermId)).SemesterGroups, ResourceTerms = await BuildResourceTerms(academicDepartmentId.Value), SelectedAcademicDepartmentId = academicDepartmentId, SelectedAcademicSemesterId = academicSemesterId, SelectedAcademicTermId = academicTermId ?? currentTermId, SelectedCourseCode = courseCode, SelectedSort = sortBy ?? "newest", SearchTerm = search });
                }

                var query = _context.PastPapers
                    .Where(p => p.Status == ResourceStatus.Approved)
                    .AsQueryable();

                Department? selectedDepartment = null;
                Semester? selectedSemester = null;

                if (!string.IsNullOrWhiteSpace(department) && !department.Equals("All", StringComparison.OrdinalIgnoreCase) && Enum.TryParse(department, true, out Department deptEnum))
                {
                    selectedDepartment = deptEnum;
                    query = query.Where(p => p.Department == deptEnum);
                }

                if (year.HasValue && year.Value > 0)
                {
                    query = query.Where(p => (int)p.Year == year.Value);
                }

                if (!string.IsNullOrWhiteSpace(semester) && !semester.Equals("All", StringComparison.OrdinalIgnoreCase) && Enum.TryParse(semester, true, out Semester semEnum))
                {
                    selectedSemester = semEnum;
                    query = query.Where(p => p.Semester == semEnum);
                }

                var browseModel = new ResourceListViewModel<PastPaper>
                {
                    SelectedDepartment = selectedDepartment?.ToString(),
                    SelectedYear = year.HasValue && year.Value > 0 ? year : null,
                    SelectedSemester = selectedSemester?.ToString(),
                    SelectedSort = sortBy ?? "newest",
                    SearchTerm = search
                };

                if (!selectedDepartment.HasValue)
                {
                    browseModel.DepartmentGroups = await query
                        .GroupBy(p => p.Department)
                        .Select(group => new ResourceBrowseGroup { Department = group.Key, Count = group.Count() })
                        .OrderBy(group => group.Department)
                        .ToListAsync();
                    return View(browseModel);
                }

                if (!selectedSemester.HasValue)
                {
                    browseModel.SemesterGroups = await query
                        .GroupBy(p => p.Semester)
                        .Select(group => new ResourceBrowseGroup { Department = selectedDepartment.Value, Semester = group.Key, Count = group.Count() })
                        .OrderBy(group => group.Semester)
                        .ToListAsync();
                    return View(browseModel);
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchTerm = search.Trim();
                    query = query.Where(p =>
                        p.Title.Contains(searchTerm) ||
                        p.SubjectName.Contains(searchTerm) ||
                        p.CourseCode.Contains(searchTerm) ||
                        p.ProfessorName.Contains(searchTerm) ||
                        p.Description != null && p.Description.Contains(searchTerm));
                }

                query = sortBy switch
                {
                    "oldest" => query.OrderBy(p => p.CreatedDate),
                    "mostDownloaded" => query.OrderByDescending(p => p.DownloadCount),
                    _ => query.OrderByDescending(p => p.CreatedDate)
                };

                var pastPapers = await query.Include(p => p.UploadedByUser).ToListAsync();

                browseModel.Resources = pastPapers;
                browseModel.CourseGroups = BuildCourseGroups(pastPapers, paper => paper.CourseCode, paper => paper.SubjectName);

                return View(browseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading past papers: {ex.Message}");
                return BadRequest("Error loading past papers.");
            }
        }

        // GET: Resource/Notes
        [HttpGet]
        public async Task<IActionResult> Notes(
            [FromQuery] string? department = null,
            [FromQuery] int? year = null,
            [FromQuery] string? semester = null,
            [FromQuery] string? sortBy = "toprated",
            [FromQuery] string? search = null,
            [FromQuery] int? academicDepartmentId = null,
            [FromQuery] int? academicSemesterId = null,
            [FromQuery] int? academicTermId = null,
            [FromQuery] string? courseCode = null)
        {
            try
            {
                ViewBag.StudySessionId = Request.Query["studySessionId"].FirstOrDefault();
                ViewBag.StudyFolderId = Request.Query["studyFolderId"].FirstOrDefault();
                if (!academicDepartmentId.HasValue && string.IsNullOrWhiteSpace(department))
                {
                    return View(await BuildNoteDepartments());
                }

                if (academicDepartmentId.HasValue && !academicSemesterId.HasValue)
                {
                    return View(await BuildNoteSemesters(academicDepartmentId.Value, await GetCurrentTermId(academicDepartmentId.Value)));
                }

                if (academicDepartmentId.HasValue && academicSemesterId.HasValue)
                {
                    var currentTermId = academicTermId.HasValue ? (academicTermId.Value > 0 ? academicTermId : null) : await GetCurrentTermId(academicDepartmentId.Value);
                    if (string.IsNullOrWhiteSpace(courseCode))
                    {
                        return View(new ResourceListViewModel<Note>
                        {
                            SelectedAcademicDepartmentId = academicDepartmentId,
                            SelectedAcademicSemesterId = academicSemesterId,
                            SelectedAcademicTermId = academicTermId ?? currentTermId,
                            SemesterGroups = (await BuildNoteSemesters(academicDepartmentId.Value, currentTermId)).SemesterGroups,
                            CourseCatalogGroups = await BuildNoteCourses(academicSemesterId.Value, currentTermId),
                            ResourceTerms = await BuildResourceTerms(academicDepartmentId.Value)
                        });
                    }
                    var dynamicQuery = _context.Notes
                        .Where(n => n.Status == ResourceStatus.Approved && n.AcademicDepartmentId == academicDepartmentId && n.AcademicSemesterId == currentTermId && n.CourseCode == courseCode);
                    dynamicQuery = ApplyNoteSearch(dynamicQuery, search);
                    dynamicQuery = sortBy switch
                    {
                        "newest" => dynamicQuery.OrderByDescending(n => n.CreatedDate),
                        "mostDownloaded" => dynamicQuery.OrderByDescending(n => n.DownloadCount),
                        _ => dynamicQuery.OrderByDescending(n => n.AverageRating)
                    };
                    var groupedNotes = await dynamicQuery.Include(n => n.UploadedByUser).ToListAsync();
                    return View(new ResourceListViewModel<Note> { Resources = groupedNotes, CourseGroups = BuildCourseGroups(groupedNotes, note => note.CourseCode, note => note.SubjectName), CourseCatalogGroups = await BuildNoteCourses(academicSemesterId.Value, currentTermId), SemesterGroups = (await BuildNoteSemesters(academicDepartmentId.Value, currentTermId)).SemesterGroups, ResourceTerms = await BuildResourceTerms(academicDepartmentId.Value), SelectedAcademicDepartmentId = academicDepartmentId, SelectedAcademicSemesterId = academicSemesterId, SelectedAcademicTermId = academicTermId ?? currentTermId, SelectedCourseCode = courseCode, SelectedSort = sortBy ?? "toprated", SearchTerm = search });
                }

                var query = _context.Notes
                    .Where(n => n.Status == ResourceStatus.Approved)
                    .AsQueryable();

                Department? selectedDepartment = null;
                Semester? selectedSemester = null;

                if (!string.IsNullOrWhiteSpace(department) && department != "All" && Enum.TryParse<Department>(department, out var deptEnum))
                {
                    selectedDepartment = deptEnum;
                    query = query.Where(n => n.Department == deptEnum);
                }

                if (year.HasValue && year.Value > 0)
                {
                    query = query.Where(n => (int)n.Year == year.Value);
                }

                if (!string.IsNullOrWhiteSpace(semester) && semester != "All" && Enum.TryParse<Semester>(semester, out var semEnum))
                {
                    selectedSemester = semEnum;
                    query = query.Where(n => n.Semester == semEnum);
                }

                var browseModel = new ResourceListViewModel<Note>
                {
                    SelectedDepartment = selectedDepartment?.ToString(),
                    SelectedYear = year.HasValue && year.Value > 0 ? year : null,
                    SelectedSemester = selectedSemester?.ToString(),
                    SelectedSort = sortBy ?? "toprated",
                    SearchTerm = search
                };

                if (!selectedDepartment.HasValue)
                {
                    browseModel.DepartmentGroups = await query
                        .GroupBy(n => n.Department)
                        .Select(group => new ResourceBrowseGroup { Department = group.Key, Count = group.Count() })
                        .OrderBy(group => group.Department)
                        .ToListAsync();
                    return View(browseModel);
                }

                if (!selectedSemester.HasValue)
                {
                    browseModel.SemesterGroups = await query
                        .GroupBy(n => n.Semester)
                        .Select(group => new ResourceBrowseGroup { Department = selectedDepartment.Value, Semester = group.Key, Count = group.Count() })
                        .OrderBy(group => group.Semester)
                        .ToListAsync();
                    return View(browseModel);
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchTerm = search.Trim();
                    query = query.Where(n =>
                        n.Title.Contains(searchTerm) ||
                        n.SubjectName.Contains(searchTerm) ||
                        n.ProfessorName != null && n.ProfessorName.Contains(searchTerm) ||
                        n.Description != null && n.Description.Contains(searchTerm));
                }

                query = sortBy switch
                {
                    "newest" => query.OrderByDescending(n => n.CreatedDate),
                    "mostDownloaded" => query.OrderByDescending(n => n.DownloadCount),
                    _ => query.OrderByDescending(n => n.AverageRating)
                };

                var notes = await query.Include(n => n.UploadedByUser).ToListAsync();

                browseModel.Resources = notes;
                browseModel.CourseGroups = BuildCourseGroups(notes, note => note.CourseCode, note => note.SubjectName);

                return View(browseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading notes: {ex.Message}");
                return BadRequest("Error loading notes.");
            }
        }

        // GET: Resource/Lectures
        [HttpGet]
        public async Task<IActionResult> Lectures(
            [FromQuery] string? department = null,
            [FromQuery] int? year = null,
            [FromQuery] string? semester = null,
            [FromQuery] string? sortBy = "newest",
            [FromQuery] string? search = null,
            [FromQuery] int? academicDepartmentId = null,
            [FromQuery] int? academicSemesterId = null,
            [FromQuery] int? academicTermId = null,
            [FromQuery] string? courseCode = null)
        {
            try
            {
                ViewBag.StudySessionId = Request.Query["studySessionId"].FirstOrDefault();
                ViewBag.StudyFolderId = Request.Query["studyFolderId"].FirstOrDefault();
                if (!academicDepartmentId.HasValue && string.IsNullOrWhiteSpace(department))
                {
                    return View(await BuildLectureDepartments());
                }

                if (academicDepartmentId.HasValue && !academicSemesterId.HasValue)
                {
                    return View(await BuildLectureSemesters(academicDepartmentId.Value, await GetCurrentTermId(academicDepartmentId.Value)));
                }

                if (academicDepartmentId.HasValue && academicSemesterId.HasValue)
                {
                    var currentTermId = academicTermId.HasValue ? (academicTermId.Value > 0 ? academicTermId : null) : await GetCurrentTermId(academicDepartmentId.Value);
                    if (string.IsNullOrWhiteSpace(courseCode))
                    {
                        return View(new ResourceListViewModel<Lecture>
                        {
                            SelectedAcademicDepartmentId = academicDepartmentId,
                            SelectedAcademicSemesterId = academicSemesterId,
                            SelectedAcademicTermId = academicTermId ?? currentTermId,
                            SemesterGroups = (await BuildLectureSemesters(academicDepartmentId.Value, currentTermId)).SemesterGroups,
                            CourseCatalogGroups = await BuildLectureCourses(academicSemesterId.Value, currentTermId),
                            ResourceTerms = await BuildResourceTerms(academicDepartmentId.Value)
                        });
                    }
                    var dynamicQuery = _context.Lectures
                        .Where(l => l.Status == ResourceStatus.Approved && l.AcademicDepartmentId == academicDepartmentId && l.AcademicSemesterId == currentTermId && l.CourseCode == courseCode);
                    dynamicQuery = ApplyLectureSearch(dynamicQuery, search);
                    dynamicQuery = sortBy switch
                    {
                        "oldest" => dynamicQuery.OrderBy(l => l.CreatedDate),
                        "mostDownloaded" => dynamicQuery.OrderByDescending(l => l.DownloadCount),
                        _ => dynamicQuery.OrderByDescending(l => l.CreatedDate)
                    };
                    var groupedLectures = await dynamicQuery.Include(l => l.UploadedByUser).ToListAsync();
                    return View(new ResourceListViewModel<Lecture> { Resources = groupedLectures, CourseGroups = BuildCourseGroups(groupedLectures, lecture => lecture.CourseCode, lecture => lecture.SubjectName), CourseCatalogGroups = await BuildLectureCourses(academicSemesterId.Value, currentTermId), SemesterGroups = (await BuildLectureSemesters(academicDepartmentId.Value, currentTermId)).SemesterGroups, ResourceTerms = await BuildResourceTerms(academicDepartmentId.Value), SelectedAcademicDepartmentId = academicDepartmentId, SelectedAcademicSemesterId = academicSemesterId, SelectedAcademicTermId = academicTermId ?? currentTermId, SelectedCourseCode = courseCode, SelectedSort = sortBy ?? "newest", SearchTerm = search });
                }

                var query = _context.Lectures
                    .Where(l => l.Status == ResourceStatus.Approved)
                    .AsQueryable();

                Department? selectedDepartment = null;
                Semester? selectedSemester = null;

                if (!string.IsNullOrWhiteSpace(department) && department != "All" && Enum.TryParse<Department>(department, out var deptEnum))
                {
                    selectedDepartment = deptEnum;
                    query = query.Where(l => l.Department == deptEnum);
                }

                if (year.HasValue && year.Value > 0)
                {
                    query = query.Where(l => (int)l.Year == year.Value);
                }

                if (!string.IsNullOrWhiteSpace(semester) && semester != "All" && Enum.TryParse<Semester>(semester, out var semEnum))
                {
                    selectedSemester = semEnum;
                    query = query.Where(l => l.Semester == semEnum);
                }

                var browseModel = new ResourceListViewModel<Lecture>
                {
                    SelectedDepartment = selectedDepartment?.ToString(),
                    SelectedYear = year.HasValue && year.Value > 0 ? year : null,
                    SelectedSemester = selectedSemester?.ToString(),
                    SelectedSort = sortBy ?? "newest",
                    SearchTerm = search
                };

                if (!selectedDepartment.HasValue)
                {
                    browseModel.DepartmentGroups = await query
                        .GroupBy(l => l.Department)
                        .Select(group => new ResourceBrowseGroup { Department = group.Key, Count = group.Count() })
                        .OrderBy(group => group.Department)
                        .ToListAsync();
                    return View(browseModel);
                }

                if (!selectedSemester.HasValue)
                {
                    browseModel.SemesterGroups = await query
                        .GroupBy(l => l.Semester)
                        .Select(group => new ResourceBrowseGroup { Department = selectedDepartment.Value, Semester = group.Key, Count = group.Count() })
                        .OrderBy(group => group.Semester)
                        .ToListAsync();
                    return View(browseModel);
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchTerm = search.Trim();
                    query = query.Where(l =>
                        l.Title.Contains(searchTerm) ||
                        l.SubjectName.Contains(searchTerm) ||
                        l.ProfessorName != null && l.ProfessorName.Contains(searchTerm) ||
                        l.Description != null && l.Description.Contains(searchTerm));
                }

                query = sortBy switch
                {
                    "oldest" => query.OrderBy(l => l.CreatedDate),
                    "mostDownloaded" => query.OrderByDescending(l => l.DownloadCount),
                    _ => query.OrderByDescending(l => l.CreatedDate)
                };

                var lectures = await query.Include(l => l.UploadedByUser).ToListAsync();

                browseModel.Resources = lectures;
                browseModel.CourseGroups = BuildCourseGroups(lectures, lecture => lecture.CourseCode, lecture => lecture.SubjectName);

                return View(browseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading lectures: {ex.Message}");
                return BadRequest("Error loading lectures.");
            }
        }

        private async Task<ResourceListViewModel<PastPaper>> BuildPastPaperDepartments()
        {
            return new ResourceListViewModel<PastPaper>
            {
                DepartmentGroups = await _context.AcademicDepartments
                    .Where(department => department.IsActive && department.University!.IsActive)
                    .OrderBy(department => department.Name)
                    .Select(department => new ResourceBrowseGroup { AcademicDepartmentId = department.Id, Name = department.Name, ImageUrl = department.ImageUrl, UniversityName = department.University!.Name, Count = _context.PastPapers.Count(paper => paper.Status == ResourceStatus.Approved && paper.AcademicDepartmentId == department.Id && paper.AcademicSemesterId == _context.AcademicSemesters.Where(term => term.DepartmentId == department.Id && term.AcademicYear == 2026 && term.TermName == "Winter" && term.IsActive).Select(term => (int?)term.Id).FirstOrDefault()) })
                    .ToListAsync()
            };
        }

        private async Task<ResourceListViewModel<PastPaper>> BuildPastPaperSemesters(int departmentId, int? termId)
        {
            return new ResourceListViewModel<PastPaper>
            {
                SelectedAcademicDepartmentId = departmentId,
                SemesterGroups = await _context.AcademicSemesters
                    .Where(semester => semester.IsActive && semester.DepartmentId == departmentId && !semester.AcademicYear.HasValue)
                    .OrderBy(semester => semester.SortOrder)
                    .Select(semester => new ResourceBrowseGroup { AcademicDepartmentId = departmentId, AcademicSemesterId = semester.Id, Name = semester.Name, Count = termId.HasValue ? _context.PastPapers.Count(paper => paper.Status == ResourceStatus.Approved && paper.AcademicSemesterId == termId && _context.AcademicCourses.Any(course => course.AcademicSemesterId == semester.Id && course.CourseCode == paper.CourseCode && course.IsActive)) : 0 })
                    .ToListAsync()
            };
        }

        private async Task<ResourceListViewModel<Note>> BuildNoteDepartments()
        {
            return new ResourceListViewModel<Note>
            {
                DepartmentGroups = await _context.AcademicDepartments
                    .Where(department => department.IsActive && department.University!.IsActive)
                    .OrderBy(department => department.Name)
                    .Select(department => new ResourceBrowseGroup { AcademicDepartmentId = department.Id, Name = department.Name, ImageUrl = department.ImageUrl, UniversityName = department.University!.Name, Count = _context.Notes.Count(note => note.Status == ResourceStatus.Approved && note.AcademicDepartmentId == department.Id && note.AcademicSemesterId == _context.AcademicSemesters.Where(term => term.DepartmentId == department.Id && term.AcademicYear == 2026 && term.TermName == "Winter" && term.IsActive).Select(term => (int?)term.Id).FirstOrDefault()) })
                    .ToListAsync()
            };
        }

        private async Task<ResourceListViewModel<Note>> BuildNoteSemesters(int departmentId, int? termId)
        {
            return new ResourceListViewModel<Note>
            {
                SelectedAcademicDepartmentId = departmentId,
                SemesterGroups = await _context.AcademicSemesters
                    .Where(semester => semester.IsActive && semester.DepartmentId == departmentId && !semester.AcademicYear.HasValue)
                    .OrderBy(semester => semester.SortOrder)
                    .Select(semester => new ResourceBrowseGroup { AcademicDepartmentId = departmentId, AcademicSemesterId = semester.Id, Name = semester.Name, Count = termId.HasValue ? _context.Notes.Count(note => note.Status == ResourceStatus.Approved && note.AcademicSemesterId == termId && _context.AcademicCourses.Any(course => course.AcademicSemesterId == semester.Id && course.CourseCode == note.CourseCode && course.IsActive)) : 0 })
                    .ToListAsync()
            };
        }

        private async Task<ResourceListViewModel<Lecture>> BuildLectureDepartments()
        {
            return new ResourceListViewModel<Lecture>
            {
                DepartmentGroups = await _context.AcademicDepartments
                    .Where(department => department.IsActive && department.University!.IsActive)
                    .OrderBy(department => department.Name)
                    .Select(department => new ResourceBrowseGroup { AcademicDepartmentId = department.Id, Name = department.Name, ImageUrl = department.ImageUrl, UniversityName = department.University!.Name, Count = _context.Lectures.Count(lecture => lecture.Status == ResourceStatus.Approved && lecture.AcademicDepartmentId == department.Id && lecture.AcademicSemesterId == _context.AcademicSemesters.Where(term => term.DepartmentId == department.Id && term.AcademicYear == 2026 && term.TermName == "Winter" && term.IsActive).Select(term => (int?)term.Id).FirstOrDefault()) })
                    .ToListAsync()
            };
        }

        private async Task<ResourceListViewModel<Lecture>> BuildLectureSemesters(int departmentId, int? termId)
        {
            return new ResourceListViewModel<Lecture>
            {
                SelectedAcademicDepartmentId = departmentId,
                SemesterGroups = await _context.AcademicSemesters
                    .Where(semester => semester.IsActive && semester.DepartmentId == departmentId && !semester.AcademicYear.HasValue)
                    .OrderBy(semester => semester.SortOrder)
                    .Select(semester => new ResourceBrowseGroup { AcademicDepartmentId = departmentId, AcademicSemesterId = semester.Id, Name = semester.Name, Count = termId.HasValue ? _context.Lectures.Count(lecture => lecture.Status == ResourceStatus.Approved && lecture.AcademicSemesterId == termId && _context.AcademicCourses.Any(course => course.AcademicSemesterId == semester.Id && course.CourseCode == lecture.CourseCode && course.IsActive)) : 0 })
                    .ToListAsync()
            };
        }

        private async Task<List<CourseCatalogGroup>> BuildPastPaperCourses(int semesterId, int? termId)
        {
            var courses = await GetCatalogCourses(semesterId);
            var result = new List<CourseCatalogGroup>();
            foreach (var course in courses)
            {
                result.Add(new CourseCatalogGroup { Id = course.Id, CourseCode = course.CourseCode, Name = course.Name, IsLab = course.IsLab, ResourceCount = termId.HasValue ? await _context.PastPapers.CountAsync(paper => paper.Status == ResourceStatus.Approved && paper.AcademicSemesterId == termId && paper.CourseCode == course.CourseCode) : 0 });
            }
            return result;
        }

        private async Task<List<CourseCatalogGroup>> BuildNoteCourses(int semesterId, int? termId)
        {
            var courses = await GetCatalogCourses(semesterId);
            var result = new List<CourseCatalogGroup>();
            foreach (var course in courses)
            {
                result.Add(new CourseCatalogGroup { Id = course.Id, CourseCode = course.CourseCode, Name = course.Name, IsLab = course.IsLab, ResourceCount = termId.HasValue ? await _context.Notes.CountAsync(note => note.Status == ResourceStatus.Approved && note.AcademicSemesterId == termId && note.CourseCode == course.CourseCode) : 0 });
            }
            return result;
        }

        private async Task<List<CourseCatalogGroup>> BuildLectureCourses(int semesterId, int? termId)
        {
            var courses = await GetCatalogCourses(semesterId);
            var result = new List<CourseCatalogGroup>();
            foreach (var course in courses)
            {
                result.Add(new CourseCatalogGroup { Id = course.Id, CourseCode = course.CourseCode, Name = course.Name, IsLab = course.IsLab, ResourceCount = termId.HasValue ? await _context.Lectures.CountAsync(lecture => lecture.Status == ResourceStatus.Approved && lecture.AcademicSemesterId == termId && lecture.CourseCode == course.CourseCode) : 0 });
            }
            return result;
        }

        private async Task<List<CourseCatalogGroup>> GetCatalogCourses(int semesterId)
        {
            var storedCourses = await _context.AcademicCourses
                .Where(course => course.AcademicSemesterId == semesterId && course.IsActive)
                .OrderBy(course => course.CourseCode)
                .Select(course => new CourseCatalogGroup { Id = course.Id, CourseCode = course.CourseCode, Name = course.Name, IsLab = course.IsLab })
                .ToListAsync();
            if (storedCourses.Count > 0) return storedCourses;

            var departmentId = await _context.AcademicSemesters.Where(semester => semester.Id == semesterId).Select(semester => semester.DepartmentId).FirstOrDefaultAsync();
            var dummyCourses = new[]
            {
                ("C101", "Programming Fundamentals", false), ("C102", "Discrete Mathematics", false),
                ("C103", "Data Structures", false), ("C104", "Digital Logic", false),
                ("C105", "Communication Skills", false), ("L101", "Programming Lab", true),
                ("L102", "Digital Systems Lab", true), ("L103", "Engineering Drawing Lab", true),
                ("L104", "Project and Design Lab", true)
            };
            return dummyCourses.Select(course => new CourseCatalogGroup { CourseCode = $"{departmentId}{course.Item1}", Name = course.Item2, IsLab = course.Item3 }).ToList();
        }

        private async Task<int?> GetCurrentTermId(int departmentId)
        {
            return await _context.AcademicSemesters
                .Where(term => term.DepartmentId == departmentId && term.AcademicYear == 2026 && term.TermName == "Winter" && term.IsActive)
                .Select(term => (int?)term.Id)
                .FirstOrDefaultAsync();
        }

        private async Task<List<ResourceTermOption>> BuildResourceTerms(int departmentId)
        {
            var terms = await _context.AcademicSemesters
                .Where(term => term.DepartmentId == departmentId && term.IsActive && term.AcademicYear.HasValue && term.AcademicYear >= 2000)
                .Select(term => new ResourceTermOption { Id = term.Id, Name = term.Name, AcademicYear = term.AcademicYear!.Value, TermName = term.TermName! })
                .ToListAsync();
            var known = terms.Select(term => $"{term.TermName}:{term.AcademicYear}").ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var year in Enumerable.Range(2000, 27).Reverse())
            {
                foreach (var termName in new[] { "Winter", "Fall", "Summer", "Spring" })
                {
                    if (!known.Contains($"{termName}:{year}")) terms.Add(new ResourceTermOption { Id = -(year * 10 + Array.IndexOf(new[] { "Winter", "Fall", "Summer", "Spring" }, termName) + 1), Name = $"{termName} {year}", AcademicYear = year, TermName = termName });
                }
            }
            return terms.OrderByDescending(term => term.AcademicYear).ThenBy(term => term.TermName switch { "Winter" => 0, "Fall" => 1, "Summer" => 2, _ => 3 }).ToList();
        }

        private static IQueryable<PastPaper> ApplyPastPaperSearch(IQueryable<PastPaper> query, string? search)
        {
            if (string.IsNullOrWhiteSpace(search)) return query;
            var term = search.Trim();
            return query.Where(paper => paper.Title.Contains(term) || paper.SubjectName.Contains(term) || paper.CourseCode.Contains(term) || paper.ProfessorName.Contains(term) || paper.Description != null && paper.Description.Contains(term));
        }

        private static IQueryable<Note> ApplyNoteSearch(IQueryable<Note> query, string? search)
        {
            if (string.IsNullOrWhiteSpace(search)) return query;
            var term = search.Trim();
            return query.Where(note => note.Title.Contains(term) || note.SubjectName.Contains(term) || note.ProfessorName != null && note.ProfessorName.Contains(term) || note.Description != null && note.Description.Contains(term));
        }

        private static IQueryable<Lecture> ApplyLectureSearch(IQueryable<Lecture> query, string? search)
        {
            if (string.IsNullOrWhiteSpace(search)) return query;
            var term = search.Trim();
            return query.Where(lecture => lecture.Title.Contains(term) || lecture.SubjectName.Contains(term) || lecture.ProfessorName != null && lecture.ProfessorName.Contains(term) || lecture.Description != null && lecture.Description.Contains(term));
        }

        private static List<ResourceCourseGroup<T>> BuildCourseGroups<T>(IEnumerable<T> resources, Func<T, string> courseCode, Func<T, string> subjectName) where T : class
        {
            return resources
                .GroupBy(resource => new { CourseCode = courseCode(resource), SubjectName = subjectName(resource) })
                .OrderBy(group => group.Key.CourseCode)
                .Select(group => new ResourceCourseGroup<T>
                {
                    CourseCode = group.Key.CourseCode,
                    SubjectName = group.Key.SubjectName,
                    Resources = group.ToList()
                })
                .ToList();
        }

        // GET: Resource/PastPaperDetails/5
        [HttpGet("Resource/PastPaperDetails/{id}")]
        public async Task<IActionResult> PastPaperDetails(int id)
        {
            try
            {
                var paper = await _context.PastPapers
                    .Include(p => p.UploadedByUser)
                    .FirstOrDefaultAsync(p => p.Id == id && p.Status == ResourceStatus.Approved);

                if (paper == null)
                {
                    return NotFound("Past paper not found.");
                }

                // Increment view count
                paper.ViewCount++;
                _context.Update(paper);
                await _context.SaveChangesAsync();

                return View(paper);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading past paper details: {ex.Message}");
                return BadRequest("Error loading past paper details.");
            }
        }

        // GET: Resource/NoteDetails/5
        [HttpGet("Resource/NoteDetails/{id}")]
        public async Task<IActionResult> NoteDetails(int id)
        {
            try
            {
                var note = await _context.Notes
                    .Include(n => n.UploadedByUser)
                    .FirstOrDefaultAsync(n => n.Id == id && n.Status == ResourceStatus.Approved);

                if (note == null)
                {
                    return NotFound("Note not found.");
                }

                // Increment view count
                note.ViewCount++;
                _context.Update(note);
                await _context.SaveChangesAsync();

                return View(note);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading note details: {ex.Message}");
                return BadRequest("Error loading note details.");
            }
        }

        // GET: Resource/LectureDetails/5
        [HttpGet("Resource/LectureDetails/{id}")]
        public async Task<IActionResult> LectureDetails(int id)
        {
            try
            {
                var lecture = await _context.Lectures
                    .Include(l => l.UploadedByUser)
                    .FirstOrDefaultAsync(l => l.Id == id && l.Status == ResourceStatus.Approved);

                if (lecture == null)
                {
                    return NotFound("Lecture not found.");
                }

                // Increment view count
                lecture.ViewCount++;
                _context.Update(lecture);
                await _context.SaveChangesAsync();

                return View(lecture);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading lecture details: {ex.Message}");
                return BadRequest("Error loading lecture details.");
            }
        }

        [HttpGet("Resource/Search")]
        public async Task<IActionResult> Search(string? term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Json(Array.Empty<object>());

            var searchTerm = term.Trim();
            var papers = await _context.PastPapers
                .Where(paper => paper.Status == ResourceStatus.Approved &&
                    (paper.Title.Contains(searchTerm) || paper.SubjectName.Contains(searchTerm) ||
                     paper.CourseCode.Contains(searchTerm) || paper.ProfessorName.Contains(searchTerm) ||
                     paper.Description != null && paper.Description.Contains(searchTerm)))
                .OrderByDescending(paper => paper.Title.StartsWith(searchTerm))
                .ThenBy(paper => paper.Title)
                .Take(8)
                .Select(paper => new { paper.Id, paper.Title, Subtitle = paper.CourseCode })
                .ToListAsync();

            var notes = await _context.Notes
                .Where(note => note.Status == ResourceStatus.Approved &&
                    (note.Title.Contains(searchTerm) || note.SubjectName.Contains(searchTerm) ||
                     note.CourseCode.Contains(searchTerm) || note.ProfessorName != null && note.ProfessorName.Contains(searchTerm) ||
                     note.Description != null && note.Description.Contains(searchTerm)))
                .OrderByDescending(note => note.Title.StartsWith(searchTerm))
                .ThenBy(note => note.Title)
                .Take(8)
                .Select(note => new { note.Id, note.Title, Subtitle = note.CourseCode })
                .ToListAsync();

            var lectures = await _context.Lectures
                .Where(lecture => lecture.Status == ResourceStatus.Approved &&
                    (lecture.Title.Contains(searchTerm) || lecture.SubjectName.Contains(searchTerm) ||
                     lecture.CourseCode.Contains(searchTerm) || lecture.ProfessorName != null && lecture.ProfessorName.Contains(searchTerm) ||
                     lecture.Description != null && lecture.Description.Contains(searchTerm)))
                .OrderByDescending(lecture => lecture.Title.StartsWith(searchTerm))
                .ThenBy(lecture => lecture.Title)
                .Take(8)
                .Select(lecture => new { lecture.Id, lecture.Title, Subtitle = lecture.CourseCode })
                .ToListAsync();

            var results = papers.Select(paper => new { type = "Past paper", title = paper.Title, subtitle = paper.Subtitle, url = Url.Action(nameof(PastPaperDetails), new { id = paper.Id }) })
                .Concat(notes.Select(note => new { type = "Study note", title = note.Title, subtitle = note.Subtitle, url = Url.Action(nameof(NoteDetails), new { id = note.Id }) }))
                .Concat(lectures.Select(lecture => new { type = "Video lecture", title = lecture.Title, subtitle = lecture.Subtitle, url = Url.Action(nameof(LectureDetails), new { id = lecture.Id }) }))
                .Take(12);

            return Json(results);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Resource/DeleteResource/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteResource(int id, string type)
        {
            string? filePath = null;
            bool validFilePath = false;

            switch (type?.ToLowerInvariant())
            {
                case "paper":
                    var paper = await _context.PastPapers.FirstOrDefaultAsync(item => item.Id == id);
                    if (paper == null) return NotFound("Past paper not found.");
                    filePath = paper.FilePath;
                    validFilePath = IsPastPaperPath(filePath);
                    _context.PastPapers.Remove(paper);
                    break;
                case "note":
                    var note = await _context.Notes.FirstOrDefaultAsync(item => item.Id == id);
                    if (note == null) return NotFound("Note not found.");
                    filePath = note.FilePath;
                    validFilePath = IsUploadPath(filePath, "notes");
                    _context.Notes.Remove(note);
                    break;
                case "lecture":
                    var lecture = await _context.Lectures.FirstOrDefaultAsync(item => item.Id == id);
                    if (lecture == null) return NotFound("Lecture not found.");
                    filePath = lecture.FilePath;
                    validFilePath = IsLecturePath(filePath);
                    _context.Lectures.Remove(lecture);
                    break;
                default:
                    return BadRequest("Invalid resource type.");
            }

            if (validFilePath && !string.IsNullOrWhiteSpace(filePath))
            {
                await _fileService.DeleteFileAsync(filePath);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Resource deleted successfully.";
            return RedirectToAction(type.Equals("paper", StringComparison.OrdinalIgnoreCase) ? nameof(PastPapers) : type.Equals("note", StringComparison.OrdinalIgnoreCase) ? nameof(Notes) : nameof(Lectures));
        }

        [HttpGet("Resource/StreamLecture/{id}")]
        public async Task<IActionResult> StreamLecture(int id)
        {
            var lecture = await _context.Lectures
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id && l.Status == ResourceStatus.Approved);

            if (lecture == null || !IsLecturePath(lecture.FilePath))
            {
                return NotFound("Lecture file not found.");
            }

            try
            {
                var extension = _fileService.GetFileExtension(lecture.FilePath);
                return File(await _fileService.GetFileStreamAsync(lecture.FilePath), _fileService.GetMimeType(extension), enableRangeProcessing: true);
            }
            catch (FileNotFoundException)
            {
                return NotFound("The lecture file could not be found.");
            }
        }

        // GET: Resource/UploadPastPaper
        [Authorize(Roles = "Admin,CR")]
        [HttpGet]
        public async Task<IActionResult> UploadPastPaper(int? academicCatalogSemesterId = null, string? courseCode = null)
        {
            var model = new UploadResourceViewModel { AcademicCatalogSemesterId = academicCatalogSemesterId, CourseCode = courseCode ?? string.Empty };
            await ApplyUploadContext(model);
            await PopulateUploadOptions(model);
            return View(model);
        }

        // POST: Resource/UploadPastPaper
        [Authorize(Roles = "Admin,CR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 50 * 1024 * 1024)]
        public async Task<IActionResult> UploadPastPaper(UploadResourceViewModel model)
        {
            await ApplyUploadContext(model);
            if (!ModelState.IsValid)
            {
                await PopulateUploadOptions(model);
                return View(model);
            }

            if (!await HasValidAcademicPlacement(model.AcademicDepartmentId, model.AcademicSemesterId, model.AcademicCourseId, model.AcademicCatalogSemesterId))
            {
                ModelState.AddModelError(nameof(model.AcademicSemesterId), "The selected semester does not belong to the selected department.");
                await PopulateUploadOptions(model);
                return View(model);
            }

            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    ModelState.AddModelError(nameof(model.File), "Please select a file to upload.");
                    await PopulateUploadOptions(model);
                    return View(model);
                }

                if (!_fileService.ValidateFileUpload(model.File, 50 * 1024 * 1024))
                {
                    ModelState.AddModelError(nameof(model.File), "Unsupported or invalid file. Use PDF, DOC, DOCX, PPT, PPTX, TXT, JPG, JPEG, PNG, or GIF up to 50MB.");
                    await PopulateUploadOptions(model);
                    return View(model);
                }

                string filePath = await _fileService.UploadFileAsync(model.File, "uploads/past-papers");

                try
                {
                    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    int uploadedByUserId = 0;
                    if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var parsedId))
                    {
                        uploadedByUserId = parsedId;
                    }
                    else
                    {
                        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                        if (user != null) uploadedByUserId = user.Id;
                    }

                    if (uploadedByUserId == 0)
                    {
                        ModelState.AddModelError("", "Could not identify current user.");
                        await _fileService.DeleteFileAsync(filePath);
                        await PopulateUploadOptions(model);
                        return View(model);
                    }

                    var paper = new PastPaper
                    {
                        Title = model.Title,
                        SubjectName = model.SubjectName,
                        CourseCode = model.CourseCode,
                        Description = model.Description,
                        ProfessorName = model.ProfessorName,
                        Department = model.Department,
                        Year = model.Year,
                        Semester = model.Semester,
                        AcademicDepartmentId = model.AcademicDepartmentId,
                        AcademicSemesterId = model.AcademicSemesterId,
                        AcademicCourseId = model.AcademicCourseId,
                        FilePath = filePath,
                        OriginalFileName = Path.GetFileName(model.File.FileName),
                        FileType = _fileService.GetFileExtension(model.File.FileName),
                        FileSizeBytes = model.File.Length,
                        UploadedByUserId = uploadedByUserId,
                        Status = User.IsInRole("Admin") ? ResourceStatus.Approved : ResourceStatus.Pending,
                        ApprovedDate = User.IsInRole("Admin") ? DateTime.UtcNow : null,
                        CreatedDate = DateTime.UtcNow
                    };

                    _context.PastPapers.Add(paper);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    await _fileService.DeleteFileAsync(filePath);
                    throw;
                }

                TempData["SuccessMessage"] = User.IsInRole("Admin")
                    ? "Past paper uploaded and approved successfully."
                    : "Past paper uploaded successfully! It will be reviewed by admins.";
                return RedirectToAction(nameof(PastPapers));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading past paper: {ex.Message}");
                ModelState.AddModelError("", "Error uploading file. Please try again.");
                await PopulateUploadOptions(model);
                return View(model);
            }
        }

        // GET: Resource/UploadNote
        [Authorize(Roles = "Admin,CR")]
        [HttpGet]
        public async Task<IActionResult> UploadNote(int? academicCatalogSemesterId = null, string? courseCode = null)
        {
            var model = new UploadNoteViewModel { AcademicCatalogSemesterId = academicCatalogSemesterId, CourseCode = courseCode ?? string.Empty };
            await ApplyUploadContext(model);
            await PopulateUploadOptions(model);
            return View(model);
        }

        // POST: Resource/UploadNote
        [Authorize(Roles = "Admin,CR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 50 * 1024 * 1024)]
        public async Task<IActionResult> UploadNote(UploadNoteViewModel model)
        {
            await ApplyUploadContext(model);
            if (!ModelState.IsValid)
            {
                await PopulateUploadOptions(model);
                return View(model);
            }

            if (!await HasValidAcademicPlacement(model.AcademicDepartmentId, model.AcademicSemesterId, model.AcademicCourseId, model.AcademicCatalogSemesterId))
            {
                ModelState.AddModelError(nameof(model.AcademicSemesterId), "The selected semester does not belong to the selected department.");
                await PopulateUploadOptions(model);
                return View(model);
            }

            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    ModelState.AddModelError(nameof(model.File), "Please select a file to upload.");
                    await PopulateUploadOptions(model);
                    return View(model);
                }

                if (!_fileService.ValidateFileUpload(model.File, 50 * 1024 * 1024))
                {
                    ModelState.AddModelError(nameof(model.File), "Unsupported or invalid file. Use PDF, DOC, DOCX, PPT, PPTX, TXT, JPG, JPEG, PNG, or GIF up to 50MB.");
                    await PopulateUploadOptions(model);
                    return View(model);
                }

                string filePath = await _fileService.UploadFileAsync(model.File, "uploads/notes");

                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int uploadedByUserId = 0;
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var parsedId))
                {
                    uploadedByUserId = parsedId;
                }
                else
                {
                    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                    if (user != null) uploadedByUserId = user.Id;
                }

                if (uploadedByUserId == 0)
                {
                    ModelState.AddModelError("", "Could not identify current user.");
                    await PopulateUploadOptions(model);
                    return View(model);
                }

                var note = new Note
                {
                    Title = model.Title,
                    SubjectName = model.SubjectName,
                    CourseCode = model.CourseCode,
                    Description = model.Description,
                    ProfessorName = model.ProfessorName,
                    Department = model.Department,
                    Year = model.Year,
                    Semester = model.Semester,
                    AcademicDepartmentId = model.AcademicDepartmentId,
                    AcademicSemesterId = model.AcademicSemesterId,
                        AcademicCourseId = model.AcademicCourseId,
                    FilePath = filePath,
                    OriginalFileName = Path.GetFileName(model.File.FileName),
                    FileType = _fileService.GetFileExtension(model.File.FileName),
                    FileSizeBytes = model.File.Length,
                    UploadedByUserId = uploadedByUserId,
                    Status = User.IsInRole("Admin") ? ResourceStatus.Approved : ResourceStatus.Pending,
                    ApprovedDate = User.IsInRole("Admin") ? DateTime.UtcNow : null,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Notes.Add(note);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Note uploaded successfully! It will be reviewed by admins.";
                return RedirectToAction(nameof(Notes));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading note: {ex.Message}");
                ModelState.AddModelError("", "Error uploading file. Please try again.");
                await PopulateUploadOptions(model);
                return View(model);
            }
        }

        // GET: Resource/UploadLecture
        [Authorize(Roles = "Admin,CR")]
        [HttpGet]
        public async Task<IActionResult> UploadLecture(int? academicCatalogSemesterId = null, string? courseCode = null)
        {
            var model = new UploadLectureViewModel { AcademicCatalogSemesterId = academicCatalogSemesterId, CourseCode = courseCode ?? string.Empty };
            await ApplyUploadContext(model);
            await PopulateUploadOptions(model);
            return View(model);
        }

        // POST: Resource/UploadLecture
        [Authorize(Roles = "Admin,CR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(long.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> UploadLecture(UploadLectureViewModel model)
        {
            await ApplyUploadContext(model);

            if (!ModelState.IsValid)
            {
                await PopulateUploadOptions(model);
                return View(model);
            }

            if (!await HasValidAcademicPlacement(model.AcademicDepartmentId, model.AcademicSemesterId, model.AcademicCourseId, model.AcademicCatalogSemesterId))
            {
                ModelState.AddModelError(nameof(model.AcademicSemesterId), "The selected semester does not belong to the selected department.");
                await PopulateUploadOptions(model);
                return View(model);
            }

            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    ModelState.AddModelError(nameof(model.File), "Please select a file to upload.");
                    await PopulateUploadOptions(model);
                    return View(model);
                }

                if (!_fileService.ValidateFileUpload(model.File, long.MaxValue))
                {
                    ModelState.AddModelError(nameof(model.File), "Unsupported or invalid lecture file. Please use a recognized video format.");
                    await PopulateUploadOptions(model);
                    return View(model);
                }

                string filePath = await _fileService.UploadFileAsync(model.File, "uploads/lectures", long.MaxValue);

                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int uploadedByUserId = 0;
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var parsedId))
                {
                    uploadedByUserId = parsedId;
                }
                else
                {
                    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                    if (user != null) uploadedByUserId = user.Id;
                }

                if (uploadedByUserId == 0)
                {
                    ModelState.AddModelError("", "Could not identify current user.");
                    await PopulateUploadOptions(model);
                    return View(model);
                }

                var lecture = new Lecture
                {
                    Title = model.Title,
                    SubjectName = model.SubjectName,
                    CourseCode = model.CourseCode,
                    Description = model.Description,
                    ProfessorName = model.ProfessorName,
                    Topics = model.Topics,
                    Department = model.Department,
                    Year = model.Year,
                    Semester = model.Semester,
                    AcademicDepartmentId = model.AcademicDepartmentId,
                    AcademicSemesterId = model.AcademicSemesterId,
                        AcademicCourseId = model.AcademicCourseId,
                    FilePath = filePath,
                    OriginalFileName = Path.GetFileName(model.File.FileName),
                    FileType = _fileService.GetFileExtension(model.File.FileName),
                    FileSizeBytes = model.File.Length,
                    UploadedByUserId = uploadedByUserId,
                    Status = User.IsInRole("Admin") ? ResourceStatus.Approved : ResourceStatus.Pending,
                    ApprovedDate = User.IsInRole("Admin") ? DateTime.UtcNow : null,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Lectures.Add(lecture);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Lecture uploaded successfully! It will be reviewed by admins.";
                return RedirectToAction(nameof(Lectures));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading lecture: {ex.Message}");
                ModelState.AddModelError("", "Error uploading file. Please try again.");
                await PopulateUploadOptions(model);
                return View(model);
            }
        }

        // GET: Resource/ViewPastPaper/5
        [HttpGet("Resource/ViewPastPaper/{id}")]
        public async Task<IActionResult> ViewPastPaper(int id)
        {
            try
            {
                var paper = await _context.PastPapers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id && p.Status == ResourceStatus.Approved);

                if (paper == null)
                {
                    return NotFound("Past paper not found.");
                }

                if (!IsPastPaperPath(paper.FilePath))
                {
                    _logger.LogWarning("Rejected unsafe Past Paper file path for resource {Id}.", id);
                    return BadRequest("The stored file path is invalid.");
                }

                var extension = _fileService.GetFileExtension(paper.FilePath);
                if (extension != ".pdf" && extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".txt")
                {
                    return RedirectToAction(nameof(Download), new { id, type = "paper" });
                }

                var stream = await _fileService.GetFileStreamAsync(paper.FilePath);
                return File(stream, _fileService.GetMimeType(extension), enableRangeProcessing: true);
            }
            catch (FileNotFoundException)
            {
                return NotFound("The stored Past Paper file could not be found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing Past Paper {Id}.", id);
                return BadRequest("Error viewing the Past Paper file.");
            }
        }

        private bool IsPastPaperPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || Path.IsPathRooted(filePath))
            {
                return false;
            }

            var webRoot = Path.GetFullPath(_fileServiceRootPath());
            var fullPath = Path.GetFullPath(Path.Combine(webRoot, filePath.Replace('/', Path.DirectorySeparatorChar)));
            var pastPaperRoot = Path.GetFullPath(Path.Combine(webRoot, "uploads", "past-papers")) + Path.DirectorySeparatorChar;
            return fullPath.StartsWith(pastPaperRoot, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsLecturePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || Path.IsPathRooted(filePath))
            {
                return false;
            }

            var webRoot = Path.GetFullPath(_fileServiceRootPath());
            var fullPath = Path.GetFullPath(Path.Combine(webRoot, filePath.Replace('/', Path.DirectorySeparatorChar)));
            var lectureRoot = Path.GetFullPath(Path.Combine(webRoot, "uploads", "lectures")) + Path.DirectorySeparatorChar;
            return fullPath.StartsWith(lectureRoot, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsUploadPath(string filePath, string folderName)
        {
            if (string.IsNullOrWhiteSpace(filePath) || Path.IsPathRooted(filePath)) return false;
            var webRoot = Path.GetFullPath(_fileServiceRootPath());
            var fullPath = Path.GetFullPath(Path.Combine(webRoot, filePath.Replace('/', Path.DirectorySeparatorChar)));
            var uploadRoot = Path.GetFullPath(Path.Combine(webRoot, "uploads", folderName)) + Path.DirectorySeparatorChar;
            return fullPath.StartsWith(uploadRoot, StringComparison.OrdinalIgnoreCase);
        }

        private string _fileServiceRootPath()
        {
            return HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().WebRootPath;
        }

        private async Task PopulateUploadOptions(UploadResourceViewModel model)
        {
            model.Departments = await _context.AcademicDepartments.Where(department => department.IsActive).OrderBy(department => department.Name).Select(department => new AcademicDepartmentOptionViewModel { Id = department.Id, UniversityId = department.UniversityId, Name = department.Name }).ToListAsync();
            model.Semesters = await _context.AcademicSemesters.Where(semester => semester.IsActive).OrderBy(semester => semester.SortOrder).Select(semester => new AcademicSemesterOptionViewModel { Id = semester.Id, DepartmentId = semester.DepartmentId, Name = semester.Name }).ToListAsync();
            model.Courses = await _context.AcademicCourses.Where(course => course.IsActive).OrderBy(course => course.CourseCode).Select(course => new AcademicCourseOptionViewModel { Id = course.Id, AcademicSemesterId = course.AcademicSemesterId, CourseCode = course.CourseCode, Name = course.Name }).ToListAsync();
        }

        private async Task PopulateUploadOptions(UploadNoteViewModel model)
        {
            model.Departments = await _context.AcademicDepartments.Where(department => department.IsActive).OrderBy(department => department.Name).Select(department => new AcademicDepartmentOptionViewModel { Id = department.Id, UniversityId = department.UniversityId, Name = department.Name }).ToListAsync();
            model.Semesters = await _context.AcademicSemesters.Where(semester => semester.IsActive).OrderBy(semester => semester.SortOrder).Select(semester => new AcademicSemesterOptionViewModel { Id = semester.Id, DepartmentId = semester.DepartmentId, Name = semester.Name }).ToListAsync();
            model.Courses = await _context.AcademicCourses.Where(course => course.IsActive).OrderBy(course => course.CourseCode).Select(course => new AcademicCourseOptionViewModel { Id = course.Id, AcademicSemesterId = course.AcademicSemesterId, CourseCode = course.CourseCode, Name = course.Name }).ToListAsync();
        }

        private async Task PopulateUploadOptions(UploadLectureViewModel model)
        {
            model.Departments = await _context.AcademicDepartments.Where(department => department.IsActive).OrderBy(department => department.Name).Select(department => new AcademicDepartmentOptionViewModel { Id = department.Id, UniversityId = department.UniversityId, Name = department.Name }).ToListAsync();
            model.Semesters = await _context.AcademicSemesters.Where(semester => semester.IsActive).OrderBy(semester => semester.SortOrder).Select(semester => new AcademicSemesterOptionViewModel { Id = semester.Id, DepartmentId = semester.DepartmentId, Name = semester.Name }).ToListAsync();
            model.Courses = await _context.AcademicCourses.Where(course => course.IsActive).OrderBy(course => course.CourseCode).Select(course => new AcademicCourseOptionViewModel { Id = course.Id, AcademicSemesterId = course.AcademicSemesterId, CourseCode = course.CourseCode, Name = course.Name }).ToListAsync();
        }

        private async Task ApplyUploadContext(UploadResourceViewModel model)
        {
            if (!model.AcademicCatalogSemesterId.HasValue || string.IsNullOrWhiteSpace(model.CourseCode)) return;
            var course = await GetUploadCourse(model.AcademicCatalogSemesterId.Value, model.CourseCode);
            if (course == null) return;
            model.AcademicCourseId = course.Id;
            model.AcademicDepartmentId = course.DepartmentId;
            model.AcademicSemesterId = await GetCurrentTermId(course.DepartmentId);
            model.CourseCode = course.CourseCode;
            model.Department = LegacyDepartment(course.DepartmentName);
            model.Year = Year.Year2026;
            model.Semester = Semester.Winter;
        }

        private async Task ApplyUploadContext(UploadNoteViewModel model)
        {
            if (!model.AcademicCatalogSemesterId.HasValue || string.IsNullOrWhiteSpace(model.CourseCode)) return;
            var course = await GetUploadCourse(model.AcademicCatalogSemesterId.Value, model.CourseCode);
            if (course == null) return;
            model.AcademicCourseId = course.Id;
            model.AcademicDepartmentId = course.DepartmentId;
            model.AcademicSemesterId = await GetCurrentTermId(course.DepartmentId);
            model.CourseCode = course.CourseCode;
            model.Department = LegacyDepartment(course.DepartmentName);
            model.Year = Year.Year2026;
            model.Semester = Semester.Winter;
        }

        private async Task ApplyUploadContext(UploadLectureViewModel model)
        {
            if (!model.AcademicCatalogSemesterId.HasValue || string.IsNullOrWhiteSpace(model.CourseCode)) return;
            var course = await GetUploadCourse(model.AcademicCatalogSemesterId.Value, model.CourseCode);
            if (course == null) return;
            model.AcademicCourseId = course.Id;
            model.AcademicDepartmentId = course.DepartmentId;
            model.AcademicSemesterId = await GetCurrentTermId(course.DepartmentId);
            model.CourseCode = course.CourseCode;
            model.Department = LegacyDepartment(course.DepartmentName);
            model.Year = Year.Year2026;
            model.Semester = Semester.Winter;
        }

        private async Task<dynamic?> GetUploadCourse(int catalogSemesterId, string courseCode)
        {
            return await _context.AcademicCourses
                .Where(course => course.AcademicSemesterId == catalogSemesterId && course.CourseCode == courseCode && course.IsActive)
                .Select(course => new { course.Id, DepartmentId = course.AcademicSemester!.DepartmentId, DepartmentName = course.AcademicSemester.Department!.Name, course.CourseCode })
                .FirstOrDefaultAsync();
        }

        private static Department LegacyDepartment(string name)
        {
            if (name.Contains("Mechanical", StringComparison.OrdinalIgnoreCase) || name.Contains("Industrial", StringComparison.OrdinalIgnoreCase) || name.Contains("Civil", StringComparison.OrdinalIgnoreCase) || name.Contains("Aeronautical", StringComparison.OrdinalIgnoreCase) || name.Contains("Engineering", StringComparison.OrdinalIgnoreCase)) return Department.Engineering;
            if (name.Contains("Data", StringComparison.OrdinalIgnoreCase) || name.Contains("Artificial", StringComparison.OrdinalIgnoreCase) || name.Contains("Software", StringComparison.OrdinalIgnoreCase)) return Department.ComputerScience;
            return Department.ComputerScience;
        }

        private Task<bool> HasValidAcademicPlacement(int? departmentId, int? semesterId, int? courseId, int? catalogSemesterId)
        {
            return _context.AcademicCourses.AnyAsync(course => course.Id == courseId && course.AcademicSemesterId == catalogSemesterId && course.AcademicSemester!.DepartmentId == departmentId && course.IsActive && course.AcademicSemester.IsActive && _context.AcademicSemesters.Any(term => term.Id == semesterId && term.DepartmentId == departmentId && term.AcademicYear == 2026 && term.TermName == "Winter" && term.IsActive));
        }

        // GET: Resource/Download/5?type=paper
        [HttpGet("Resource/Download/{id}")]
        public async Task<IActionResult> Download(int id, string type)
        {
            try
            {
                if (type != "paper" && type != "note" && type != "lecture")
                {
                    return BadRequest("Invalid resource type.");
                }

                if (type == "paper")
                {
                    var paper = await _context.PastPapers.FirstOrDefaultAsync(p => p.Id == id);
                    if (paper == null)
                    {
                        return NotFound("Past paper not found.");
                    }

                    if (!IsPastPaperPath(paper.FilePath))
                    {
                        _logger.LogWarning("Rejected unsafe Past Paper file path for resource {Id}.", id);
                        return BadRequest("The stored file path is invalid.");
                    }

                    // Increment download count
                    paper.DownloadCount++;
                    _context.Update(paper);
                    await _context.SaveChangesAsync();

                    var stream = await _fileService.GetFileStreamAsync(paper.FilePath);
                    string extension = _fileService.GetFileExtension(paper.OriginalFileName);
                    string mimeType = _fileService.GetMimeType(extension);
                    string downloadName = Path.GetFileName(paper.OriginalFileName);
                    return File(stream, mimeType, downloadName);
                }
                else if (type == "note")
                {
                    var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
                    if (note == null)
                    {
                        return NotFound("Note not found.");
                    }

                    // Increment download count
                    note.DownloadCount++;
                    _context.Update(note);
                    await _context.SaveChangesAsync();

                    var stream = await _fileService.GetFileStreamAsync(note.FilePath);
                    string mimeType = _fileService.GetMimeType(note.FileType!);
                    return File(stream, mimeType, $"{note.CourseCode}_{note.Title}.{note.FileType!.TrimStart('.')}");
                }
                else
                {
                    var lecture = await _context.Lectures.FirstOrDefaultAsync(l => l.Id == id);
                    if (lecture == null)
                    {
                        return NotFound("Lecture not found.");
                    }

                    // Increment download count
                    lecture.DownloadCount++;
                    _context.Update(lecture);
                    await _context.SaveChangesAsync();

                    var stream = await _fileService.GetFileStreamAsync(lecture.FilePath);
                    string mimeType = _fileService.GetMimeType(lecture.FileType!);
                    return File(stream, mimeType, $"{lecture.CourseCode}_{lecture.Title}.{lecture.FileType!.TrimStart('.')}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error downloading resource: {ex.Message}");
                return BadRequest("Error downloading file.");
            }
        }

        // POST: Resource/RateNote
        [Authorize(Roles = "Student,Admin")]
        [HttpPost]
        public async Task<IActionResult> RateNote(int id, int rating)
        {
            try
            {
                if (rating < 1 || rating > 5)
                {
                    return BadRequest("Rating must be between 1 and 5.");
                }

                var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
                if (note == null)
                {
                    return NotFound("Note not found.");
                }

                // Update rating (simple average)
                note.TotalRatings++;
                note.AverageRating = ((note.AverageRating * (note.TotalRatings - 1)) + rating) / note.TotalRatings;

                _context.Update(note);
                await _context.SaveChangesAsync();

                return Json(new { success = true, newRating = note.AverageRating });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error rating note: {ex.Message}");
                return BadRequest("Error rating note.");
            }
        }
    }
}
