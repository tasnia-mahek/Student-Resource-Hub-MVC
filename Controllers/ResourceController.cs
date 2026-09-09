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
            [FromQuery] string? search = null)
        {
            try
            {
                var query = _context.PastPapers
                    .Where(p => p.Status == ResourceStatus.Approved)
                    .AsQueryable();

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

                if (!string.IsNullOrWhiteSpace(department) && !department.Equals("All", StringComparison.OrdinalIgnoreCase) && Enum.TryParse(department, true, out Department deptEnum))
                {
                    query = query.Where(p => p.Department == deptEnum);
                }

                if (year.HasValue && year.Value > 0)
                {
                    query = query.Where(p => (int)p.Year == year.Value);
                }

                if (!string.IsNullOrWhiteSpace(semester) && !semester.Equals("All", StringComparison.OrdinalIgnoreCase) && Enum.TryParse(semester, true, out Semester semEnum))
                {
                    query = query.Where(p => p.Semester == semEnum);
                }

                query = sortBy switch
                {
                    "oldest" => query.OrderBy(p => p.CreatedDate),
                    "mostDownloaded" => query.OrderByDescending(p => p.DownloadCount),
                    _ => query.OrderByDescending(p => p.CreatedDate)
                };

                var pastPapers = await query.Include(p => p.UploadedByUser).ToListAsync();

                var viewModel = new ResourceListViewModel<PastPaper>
                {
                    Resources = pastPapers,
                    SelectedDepartment = string.IsNullOrWhiteSpace(department) || department == "All" ? null : department,
                    SelectedYear = year.HasValue && year.Value > 0 ? year : null,
                    SelectedSemester = string.IsNullOrWhiteSpace(semester) || semester == "All" ? null : semester,
                    SelectedSort = sortBy ?? "newest",
                    SearchTerm = search
                };

                return View(viewModel);
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
            [FromQuery] string? search = null)
        {
            try
            {
                var query = _context.Notes
                    .Where(n => n.Status == ResourceStatus.Approved)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchTerm = search.Trim();
                    query = query.Where(n =>
                        n.Title.Contains(searchTerm) ||
                        n.SubjectName.Contains(searchTerm) ||
                        n.ProfessorName != null && n.ProfessorName.Contains(searchTerm) ||
                        n.Description != null && n.Description.Contains(searchTerm));
                }

                if (!string.IsNullOrWhiteSpace(department) && department != "All" && Enum.TryParse<Department>(department, out var deptEnum))
                {
                    query = query.Where(n => n.Department == deptEnum);
                }

                if (year.HasValue && year.Value > 0)
                {
                    query = query.Where(n => (int)n.Year == year.Value);
                }

                if (!string.IsNullOrWhiteSpace(semester) && semester != "All" && Enum.TryParse<Semester>(semester, out var semEnum))
                {
                    query = query.Where(n => n.Semester == semEnum);
                }

                query = sortBy switch
                {
                    "newest" => query.OrderByDescending(n => n.CreatedDate),
                    "mostDownloaded" => query.OrderByDescending(n => n.DownloadCount),
                    _ => query.OrderByDescending(n => n.AverageRating)
                };

                var notes = await query.Include(n => n.UploadedByUser).ToListAsync();

                var viewModel = new ResourceListViewModel<Note>
                {
                    Resources = notes,
                    SelectedDepartment = string.IsNullOrWhiteSpace(department) || department == "All" ? null : department,
                    SelectedYear = year.HasValue && year.Value > 0 ? year : null,
                    SelectedSemester = string.IsNullOrWhiteSpace(semester) || semester == "All" ? null : semester,
                    SelectedSort = sortBy ?? "toprated",
                    SearchTerm = search
                };

                return View(viewModel);
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
            [FromQuery] string? search = null)
        {
            try
            {
                var query = _context.Lectures
                    .Where(l => l.Status == ResourceStatus.Approved)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchTerm = search.Trim();
                    query = query.Where(l =>
                        l.Title.Contains(searchTerm) ||
                        l.SubjectName.Contains(searchTerm) ||
                        l.ProfessorName != null && l.ProfessorName.Contains(searchTerm) ||
                        l.Description != null && l.Description.Contains(searchTerm));
                }

                if (!string.IsNullOrWhiteSpace(department) && department != "All" && Enum.TryParse<Department>(department, out var deptEnum))
                {
                    query = query.Where(l => l.Department == deptEnum);
                }

                if (year.HasValue && year.Value > 0)
                {
                    query = query.Where(l => (int)l.Year == year.Value);
                }

                if (!string.IsNullOrWhiteSpace(semester) && semester != "All" && Enum.TryParse<Semester>(semester, out var semEnum))
                {
                    query = query.Where(l => l.Semester == semEnum);
                }

                query = sortBy switch
                {
                    "oldest" => query.OrderBy(l => l.CreatedDate),
                    "mostDownloaded" => query.OrderByDescending(l => l.DownloadCount),
                    _ => query.OrderByDescending(l => l.CreatedDate)
                };

                var lectures = await query.Include(l => l.UploadedByUser).ToListAsync();

                var viewModel = new ResourceListViewModel<Lecture>
                {
                    Resources = lectures,
                    SelectedDepartment = string.IsNullOrWhiteSpace(department) || department == "All" ? null : department,
                    SelectedYear = year.HasValue && year.Value > 0 ? year : null,
                    SelectedSemester = string.IsNullOrWhiteSpace(semester) || semester == "All" ? null : semester,
                    SelectedSort = sortBy ?? "newest",
                    SearchTerm = search
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading lectures: {ex.Message}");
                return BadRequest("Error loading lectures.");
            }
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

        // GET: Resource/UploadPastPaper
        [Authorize(Roles = "Student,Admin")]
        [HttpGet]
        public IActionResult UploadPastPaper()
        {
            return View(new UploadResourceViewModel());
        }

        // POST: Resource/UploadPastPaper
        [Authorize(Roles = "Student,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPastPaper(UploadResourceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    ModelState.AddModelError(nameof(model.File), "Please select a file to upload.");
                    return View(model);
                }

                if (!_fileService.ValidateFileUpload(model.File, 50 * 1024 * 1024))
                {
                    ModelState.AddModelError(nameof(model.File), "Unsupported or invalid file. Use PDF, DOC, DOCX, PPT, PPTX, TXT, JPG, JPEG, PNG, or GIF up to 50MB.");
                    return View(model);
                }

                string filePath = await _fileService.UploadFileAsync(model.File, "uploads/past-papers");

                try
                {
                    var identityValues = User.Claims
                        .Select(claim => claim.Value)
                        .Where(value => !string.IsNullOrWhiteSpace(value))
                        .Distinct()
                        .ToArray();
                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => identityValues.Contains(u.Email) || identityValues.Contains(u.FullName));
                    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.Id.ToString();

                    if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var uploadedByUserId))
                    {
                        ModelState.AddModelError("", "Could not identify current user.");
                        await _fileService.DeleteFileAsync(filePath);
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
                return View(model);
            }
        }

        // GET: Resource/UploadNote
        [Authorize(Roles = "Student,Admin")]
        [HttpGet]
        public IActionResult UploadNote()
        {
            return View(new UploadNoteViewModel());
        }

        // POST: Resource/UploadNote
        [Authorize(Roles = "Student,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadNote(UploadNoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    ModelState.AddModelError(nameof(model.File), "Please select a file to upload.");
                    return View(model);
                }

                if (!_fileService.ValidateFileUpload(model.File, 50 * 1024 * 1024))
                {
                    ModelState.AddModelError(nameof(model.File), "Unsupported or invalid file. Use PDF, DOC, DOCX, PPT, PPTX, TXT, JPG, JPEG, PNG, or GIF up to 50MB.");
                    return View(model);
                }

                string filePath = await _fileService.UploadFileAsync(model.File, "uploads/notes");

                var userId = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ??
                             _context.Users.FirstOrDefault(u => u.Email == User.Identity!.Name)?.Id.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError("", "Could not identify current user.");
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
                    FilePath = filePath,
                    OriginalFileName = Path.GetFileName(model.File.FileName),
                    FileType = _fileService.GetFileExtension(model.File.FileName),
                    FileSizeBytes = model.File.Length,
                    UploadedByUserId = int.Parse(userId),
                    Status = ResourceStatus.Pending,
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
                return View(model);
            }
        }

        // GET: Resource/UploadLecture
        [Authorize(Roles = "Student,Admin")]
        [HttpGet]
        public IActionResult UploadLecture()
        {
            return View(new UploadLectureViewModel());
        }

        // POST: Resource/UploadLecture
        [Authorize(Roles = "Student,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadLecture(UploadLectureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    ModelState.AddModelError(nameof(model.File), "Please select a file to upload.");
                    return View(model);
                }

                if (!_fileService.ValidateFileUpload(model.File, 50 * 1024 * 1024))
                {
                    ModelState.AddModelError(nameof(model.File), "Unsupported or invalid file. Use PDF, DOC, DOCX, PPT, PPTX, TXT, JPG, JPEG, PNG, or GIF up to 50MB.");
                    return View(model);
                }

                string filePath = await _fileService.UploadFileAsync(model.File, "uploads/lectures");

                var userId = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ??
                             _context.Users.FirstOrDefault(u => u.Email == User.Identity!.Name)?.Id.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError("", "Could not identify current user.");
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
                    FilePath = filePath,
                    OriginalFileName = Path.GetFileName(model.File.FileName),
                    FileType = _fileService.GetFileExtension(model.File.FileName),
                    FileSizeBytes = model.File.Length,
                    UploadedByUserId = int.Parse(userId),
                    Status = ResourceStatus.Pending,
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

        private string _fileServiceRootPath()
        {
            return HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().WebRootPath;
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
