using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            [FromQuery] string? sortBy = "newest")
        {
            try
            {
                var query = _context.PastPapers
                    .Where(p => p.Status == ResourceStatus.Approved)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(department) && Enum.TryParse<Department>(department, out var deptEnum))
                {
                    query = query.Where(p => p.Department == deptEnum);
                }

                if (year.HasValue)
                {
                    query = query.Where(p => (int)p.Year == year.Value);
                }

                if (!string.IsNullOrEmpty(semester) && Enum.TryParse<Semester>(semester, out var semEnum))
                {
                    query = query.Where(p => p.Semester == semEnum);
                }

                // Apply sorting
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
                    SelectedDepartment = department,
                    SelectedYear = year,
                    SelectedSemester = semester,
                    SelectedSort = sortBy ?? "newest"
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
            [FromQuery] string? sortBy = "toprated")
        {
            try
            {
                var query = _context.Notes
                    .Where(n => n.Status == ResourceStatus.Approved)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(department) && Enum.TryParse<Department>(department, out var deptEnum))
                {
                    query = query.Where(n => n.Department == deptEnum);
                }

                if (year.HasValue)
                {
                    query = query.Where(n => (int)n.Year == year.Value);
                }

                if (!string.IsNullOrEmpty(semester) && Enum.TryParse<Semester>(semester, out var semEnum))
                {
                    query = query.Where(n => n.Semester == semEnum);
                }

                // Apply sorting
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
                    SelectedDepartment = department,
                    SelectedYear = year,
                    SelectedSemester = semester,
                    SelectedSort = sortBy ?? "toprated"
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading notes: {ex.Message}");
                return BadRequest("Error loading notes.");
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

        // GET: Resource/UploadPastPaper
        [Authorize(Roles = "Student,Admin")]
        [HttpGet]
        public IActionResult UploadPastPaper()
        {
            return View();
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

                // Upload file
                string filePath = await _fileService.UploadFileAsync(model.File, "uploads/past-papers");

                var userId = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ??
                             _context.Users.FirstOrDefault(u => u.Email == User.Identity!.Name)?.Id.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError("", "Could not identify current user.");
                    return View(model);
                }

                var paper = new PastPaper
                {
                    SubjectName = model.SubjectName,
                    CourseCode = model.CourseCode,
                    Description = model.Description,
                    ProfessorName = model.ProfessorName,
                    Department = model.Department,
                    Year = model.Year,
                    Semester = model.Semester,
                    FilePath = filePath,
                    FileType = _fileService.GetFileExtension(model.File.FileName),
                    FileSizeBytes = model.File.Length,
                    UploadedByUserId = int.Parse(userId),
                    Status = ResourceStatus.Pending,
                    CreatedDate = DateTime.UtcNow
                };

                _context.PastPapers.Add(paper);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Past paper uploaded successfully! It will be reviewed by admins.";
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
            return View();
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

                // Upload file
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

        // GET: Resource/Download/5?type=paper
        [HttpGet("Resource/Download/{id}")]
        public async Task<IActionResult> Download(int id, string type)
        {
            try
            {
                if (type != "paper" && type != "note")
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

                    // Increment download count
                    paper.DownloadCount++;
                    _context.Update(paper);
                    await _context.SaveChangesAsync();

                    var stream = await _fileService.GetFileStreamAsync(paper.FilePath);
                    string mimeType = _fileService.GetMimeType(paper.FileType!);
                    return File(stream, mimeType, $"{paper.CourseCode}_{paper.Year}.{paper.FileType!.TrimStart('.')}");
                }
                else
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
