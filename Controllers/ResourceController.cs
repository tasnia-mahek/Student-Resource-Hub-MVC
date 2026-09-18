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
            [FromQuery] int? academicSemesterId = null)
        {
            try
            {
                if (!academicDepartmentId.HasValue && string.IsNullOrWhiteSpace(department))
                {
                    return View(await BuildPastPaperDepartments());
                }

                if (academicDepartmentId.HasValue && !academicSemesterId.HasValue)
                {
                    return View(await BuildPastPaperSemesters(academicDepartmentId.Value));
                }

                if (academicDepartmentId.HasValue && academicSemesterId.HasValue)
                {
                    var dynamicQuery = _context.PastPapers
                        .Where(p => p.Status == ResourceStatus.Approved && p.AcademicDepartmentId == academicDepartmentId && p.AcademicSemesterId == academicSemesterId);
                    dynamicQuery = ApplyPastPaperSearch(dynamicQuery, search);
                    dynamicQuery = sortBy switch
                    {
                        "oldest" => dynamicQuery.OrderBy(p => p.CreatedDate),
                        "mostDownloaded" => dynamicQuery.OrderByDescending(p => p.DownloadCount),
                        _ => dynamicQuery.OrderByDescending(p => p.CreatedDate)
                    };
                    var papers = await dynamicQuery.Include(p => p.UploadedByUser).ToListAsync();
                    return View(new ResourceListViewModel<PastPaper> { Resources = papers, CourseGroups = BuildCourseGroups(papers, paper => paper.CourseCode, paper => paper.SubjectName), SelectedAcademicDepartmentId = academicDepartmentId, SelectedAcademicSemesterId = academicSemesterId, SelectedSort = sortBy ?? "newest", SearchTerm = search });
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
            [FromQuery] int? academicSemesterId = null)
        {
            try
            {
                if (!academicDepartmentId.HasValue && string.IsNullOrWhiteSpace(department))
                {
                    return View(await BuildNoteDepartments());
                }

                if (academicDepartmentId.HasValue && !academicSemesterId.HasValue)
                {
                    return View(await BuildNoteSemesters(academicDepartmentId.Value));
                }

                if (academicDepartmentId.HasValue && academicSemesterId.HasValue)
                {
                    var dynamicQuery = _context.Notes
                        .Where(n => n.Status == ResourceStatus.Approved && n.AcademicDepartmentId == academicDepartmentId && n.AcademicSemesterId == academicSemesterId);
                    dynamicQuery = ApplyNoteSearch(dynamicQuery, search);
                    dynamicQuery = sortBy switch
                    {
                        "newest" => dynamicQuery.OrderByDescending(n => n.CreatedDate),
                        "mostDownloaded" => dynamicQuery.OrderByDescending(n => n.DownloadCount),
                        _ => dynamicQuery.OrderByDescending(n => n.AverageRating)
                    };
                    var groupedNotes = await dynamicQuery.Include(n => n.UploadedByUser).ToListAsync();
                    return View(new ResourceListViewModel<Note> { Resources = groupedNotes, CourseGroups = BuildCourseGroups(groupedNotes, note => note.CourseCode, note => note.SubjectName), SelectedAcademicDepartmentId = academicDepartmentId, SelectedAcademicSemesterId = academicSemesterId, SelectedSort = sortBy ?? "toprated", SearchTerm = search });
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
            [FromQuery] int? academicSemesterId = null)
        {
            try
            {
                if (!academicDepartmentId.HasValue && string.IsNullOrWhiteSpace(department))
                {
                    return View(await BuildLectureDepartments());
                }

                if (academicDepartmentId.HasValue && !academicSemesterId.HasValue)
                {
                    return View(await BuildLectureSemesters(academicDepartmentId.Value));
                }

                if (academicDepartmentId.HasValue && academicSemesterId.HasValue)
                {
                    var dynamicQuery = _context.Lectures
                        .Where(l => l.Status == ResourceStatus.Approved && l.AcademicDepartmentId == academicDepartmentId && l.AcademicSemesterId == academicSemesterId);
                    dynamicQuery = ApplyLectureSearch(dynamicQuery, search);
                    dynamicQuery = sortBy switch
                    {
                        "oldest" => dynamicQuery.OrderBy(l => l.CreatedDate),
                        "mostDownloaded" => dynamicQuery.OrderByDescending(l => l.DownloadCount),
                        _ => dynamicQuery.OrderByDescending(l => l.CreatedDate)
                    };
                    var groupedLectures = await dynamicQuery.Include(l => l.UploadedByUser).ToListAsync();
                    return View(new ResourceListViewModel<Lecture> { Resources = groupedLectures, CourseGroups = BuildCourseGroups(groupedLectures, lecture => lecture.CourseCode, lecture => lecture.SubjectName), SelectedAcademicDepartmentId = academicDepartmentId, SelectedAcademicSemesterId = academicSemesterId, SelectedSort = sortBy ?? "newest", SearchTerm = search });
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
                    .Select(department => new ResourceBrowseGroup { AcademicDepartmentId = department.Id, Name = department.Name, ImageUrl = department.ImageUrl, UniversityName = department.University!.Name, Count = _context.PastPapers.Count(paper => paper.Status == ResourceStatus.Approved && paper.AcademicDepartmentId == department.Id) })
                    .ToListAsync()
            };
        }

        private async Task<ResourceListViewModel<PastPaper>> BuildPastPaperSemesters(int departmentId)
        {
            return new ResourceListViewModel<PastPaper>
            {
                SelectedAcademicDepartmentId = departmentId,
                SemesterGroups = await _context.AcademicSemesters
                    .Where(semester => semester.IsActive && semester.DepartmentId == departmentId)
                    .OrderBy(semester => semester.SortOrder)
                    .Select(semester => new ResourceBrowseGroup { AcademicDepartmentId = departmentId, AcademicSemesterId = semester.Id, Name = semester.Name, Count = _context.PastPapers.Count(paper => paper.Status == ResourceStatus.Approved && paper.AcademicSemesterId == semester.Id) })
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
                    .Select(department => new ResourceBrowseGroup { AcademicDepartmentId = department.Id, Name = department.Name, ImageUrl = department.ImageUrl, UniversityName = department.University!.Name, Count = _context.Notes.Count(note => note.Status == ResourceStatus.Approved && note.AcademicDepartmentId == department.Id) })
                    .ToListAsync()
            };
        }

        private async Task<ResourceListViewModel<Note>> BuildNoteSemesters(int departmentId)
        {
            return new ResourceListViewModel<Note>
            {
                SelectedAcademicDepartmentId = departmentId,
                SemesterGroups = await _context.AcademicSemesters
                    .Where(semester => semester.IsActive && semester.DepartmentId == departmentId)
                    .OrderBy(semester => semester.SortOrder)
                    .Select(semester => new ResourceBrowseGroup { AcademicDepartmentId = departmentId, AcademicSemesterId = semester.Id, Name = semester.Name, Count = _context.Notes.Count(note => note.Status == ResourceStatus.Approved && note.AcademicSemesterId == semester.Id) })
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
                    .Select(department => new ResourceBrowseGroup { AcademicDepartmentId = department.Id, Name = department.Name, ImageUrl = department.ImageUrl, UniversityName = department.University!.Name, Count = _context.Lectures.Count(lecture => lecture.Status == ResourceStatus.Approved && lecture.AcademicDepartmentId == department.Id) })
                    .ToListAsync()
            };
        }

        private async Task<ResourceListViewModel<Lecture>> BuildLectureSemesters(int departmentId)
        {
            return new ResourceListViewModel<Lecture>
            {
                SelectedAcademicDepartmentId = departmentId,
                SemesterGroups = await _context.AcademicSemesters
                    .Where(semester => semester.IsActive && semester.DepartmentId == departmentId)
                    .OrderBy(semester => semester.SortOrder)
                    .Select(semester => new ResourceBrowseGroup { AcademicDepartmentId = departmentId, AcademicSemesterId = semester.Id, Name = semester.Name, Count = _context.Lectures.Count(lecture => lecture.Status == ResourceStatus.Approved && lecture.AcademicSemesterId == semester.Id) })
                    .ToListAsync()
            };
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
        public async Task<IActionResult> UploadPastPaper()
        {
            var model = new UploadResourceViewModel();
            await PopulateUploadOptions(model);
            return View(model);
        }

        // POST: Resource/UploadPastPaper
        [Authorize(Roles = "Admin,CR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPastPaper(UploadResourceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateUploadOptions(model);
                return View(model);
            }

            if (!await HasValidAcademicPlacement(model.AcademicDepartmentId, model.AcademicSemesterId))
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
        public async Task<IActionResult> UploadNote()
        {
            var model = new UploadNoteViewModel();
            await PopulateUploadOptions(model);
            return View(model);
        }

        // POST: Resource/UploadNote
        [Authorize(Roles = "Admin,CR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadNote(UploadNoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateUploadOptions(model);
                return View(model);
            }

            if (!await HasValidAcademicPlacement(model.AcademicDepartmentId, model.AcademicSemesterId))
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UploadLecture()
        {
            var model = new UploadLectureViewModel();
            await PopulateUploadOptions(model);
            return View(model);
        }

        // POST: Resource/UploadLecture
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(long.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> UploadLecture(UploadLectureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateUploadOptions(model);
                return View(model);
            }

            if (!await HasValidAcademicPlacement(model.AcademicDepartmentId, model.AcademicSemesterId))
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

        private string _fileServiceRootPath()
        {
            return HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().WebRootPath;
        }

        private async Task PopulateUploadOptions(UploadResourceViewModel model)
        {
            model.Departments = await _context.AcademicDepartments.Where(department => department.IsActive).OrderBy(department => department.Name).Select(department => new AcademicDepartmentOptionViewModel { Id = department.Id, UniversityId = department.UniversityId, Name = department.Name }).ToListAsync();
            model.Semesters = await _context.AcademicSemesters.Where(semester => semester.IsActive).OrderBy(semester => semester.SortOrder).Select(semester => new AcademicSemesterOptionViewModel { Id = semester.Id, DepartmentId = semester.DepartmentId, Name = semester.Name }).ToListAsync();
        }

        private async Task PopulateUploadOptions(UploadNoteViewModel model)
        {
            model.Departments = await _context.AcademicDepartments.Where(department => department.IsActive).OrderBy(department => department.Name).Select(department => new AcademicDepartmentOptionViewModel { Id = department.Id, UniversityId = department.UniversityId, Name = department.Name }).ToListAsync();
            model.Semesters = await _context.AcademicSemesters.Where(semester => semester.IsActive).OrderBy(semester => semester.SortOrder).Select(semester => new AcademicSemesterOptionViewModel { Id = semester.Id, DepartmentId = semester.DepartmentId, Name = semester.Name }).ToListAsync();
        }

        private async Task PopulateUploadOptions(UploadLectureViewModel model)
        {
            model.Departments = await _context.AcademicDepartments.Where(department => department.IsActive).OrderBy(department => department.Name).Select(department => new AcademicDepartmentOptionViewModel { Id = department.Id, UniversityId = department.UniversityId, Name = department.Name }).ToListAsync();
            model.Semesters = await _context.AcademicSemesters.Where(semester => semester.IsActive).OrderBy(semester => semester.SortOrder).Select(semester => new AcademicSemesterOptionViewModel { Id = semester.Id, DepartmentId = semester.DepartmentId, Name = semester.Name }).ToListAsync();
        }

        private Task<bool> HasValidAcademicPlacement(int? departmentId, int? semesterId)
        {
            return _context.AcademicSemesters.AnyAsync(semester => semester.Id == semesterId && semester.DepartmentId == departmentId && semester.IsActive);
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
