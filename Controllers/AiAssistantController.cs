using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using student_resource_hub.Data;
using student_resource_hub.Models;
using student_resource_hub.ViewModels;

namespace student_resource_hub.Controllers
{
    [Authorize]
    public class AiAssistantController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AiAssistantController> _logger;

        private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "a", "about", "all", "an", "and", "are", "as", "at", "available", "be", "by", "can",
            "could", "course", "courses", "do", "does", "exam", "exams", "final", "find", "for",
            "from", "get", "give", "have", "help", "how", "i", "in", "is", "it", "lecture",
            "lectures", "material", "materials", "me", "midterm", "need", "note", "notes", "of",
            "on", "or", "our", "paper", "papers", "past", "please", "resource", "resources",
            "search", "show", "slide", "slides", "some", "tell", "the", "there", "to", "what",
            "where", "which", "with", "you", "your"
        };

        public AiAssistantController(
            ApplicationDbContext context,
            ILogger<AiAssistantController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /AiAssistant
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /AiAssistant/Query or /AiAssistant/Ask
        [AllowAnonymous]
        [HttpPost]
        [Route("AiAssistant/Query")]
        [Route("AiAssistant/Ask")]
        public async Task<IActionResult> Query([FromBody] AiQueryRequest? request)
        {
            var rawQuery = request?.Query?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(rawQuery) && Request.HasFormContentType && Request.Form.ContainsKey("query"))
            {
                rawQuery = Request.Form["query"].ToString().Trim();
            }

            if (string.IsNullOrWhiteSpace(rawQuery) && Request.Query.ContainsKey("query"))
            {
                rawQuery = Request.Query["query"].ToString().Trim();
            }

            if (string.IsNullOrWhiteSpace(rawQuery))
            {
                return Json(new AiQueryResponse
                {
                    Success = true,
                    Query = string.Empty,
                    Reply = "<p>Please enter a question or topic so I can search our course materials for you.</p>",
                    TotalMatches = 0
                });
            }

            try
            {
                var response = await ProcessAcademicQueryAsync(rawQuery);
                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing AI Assistant search for query: {Query}", rawQuery);
                return Json(new AiQueryResponse
                {
                    Success = false,
                    Query = rawQuery,
                    Reply = "<p>An unexpected error occurred while searching the database. Please try again shortly.</p>",
                    TotalMatches = 0
                });
            }
        }

        private async Task<AiQueryResponse> ProcessAcademicQueryAsync(string query)
        {
            var response = new AiQueryResponse
            {
                Success = true,
                Query = query
            };

            // 1. Check for Starter Kit intent
            if (IsStarterKitQuery(query))
            {
                response.Reply = BuildStarterKitReply();
                return response;
            }

            // 2. Check for Study Tips intent
            if (IsStudyTipsQuery(query))
            {
                response.Reply = BuildStudyTipsReply();
                return response;
            }

            // 3. Extract course code if present
            var courseCodeMatch = Regex.Match(query, @"(?i)\b([A-Za-z]{2,4})\s*[-]?\s*(\d{3,4}[A-Za-z]?)\b");
            string? deptLetters = null;
            string? codeNumber = null;
            string? canonicalCode = null;
            string? compactCode = null;

            if (courseCodeMatch.Success)
            {
                deptLetters = courseCodeMatch.Groups[1].Value.ToUpperInvariant();
                codeNumber = courseCodeMatch.Groups[2].Value.ToUpperInvariant();
                canonicalCode = $"{deptLetters} {codeNumber}";
                compactCode = $"{deptLetters}{codeNumber}";
            }
            else
            {
                var numOnlyMatch = Regex.Match(query, @"\b\d{3,4}\b");
                if (numOnlyMatch.Success)
                {
                    codeNumber = numOnlyMatch.Value;
                    canonicalCode = codeNumber;
                    compactCode = codeNumber;
                }
            }

            // 4. Extract subject/topic keywords
            var keyword = ExtractKeyword(query, canonicalCode, compactCode);

            // 5. Detect resource category preference
            bool wantsPastPapers = ContainsAny(query, "past paper", "past papers", "pastpaper", "pastpapers", "question paper", "question papers", "previous paper", "exam paper", "exam papers", "midterm", "final exam");
            bool wantsNotes = ContainsAny(query, "note", "notes", "handout", "handouts", "lecture note", "study note");
            bool wantsLectures = ContainsAny(query, "lecture", "lectures", "slide", "slides", "recording", "video");

            // 6. Route search based on intent
            if (wantsPastPapers && !wantsNotes && !wantsLectures)
            {
                await SearchPastPapersAsync(response, canonicalCode, compactCode, deptLetters, codeNumber, keyword);
            }
            else if (wantsNotes && !wantsPastPapers && !wantsLectures)
            {
                await SearchNotesAsync(response, canonicalCode, compactCode, deptLetters, codeNumber, keyword);
            }
            else if (wantsLectures && !wantsPastPapers && !wantsNotes)
            {
                await SearchLecturesAsync(response, canonicalCode, compactCode, deptLetters, codeNumber, keyword);
            }
            else
            {
                // General or multi-category search
                await SearchAllResourcesAsync(response, canonicalCode, compactCode, deptLetters, codeNumber, keyword);
            }

            return response;
        }

        private async Task SearchPastPapersAsync(
            AiQueryResponse response,
            string? canonicalCode,
            string? compactCode,
            string? deptLetters,
            string? codeNumber,
            string? keyword)
        {
            var targetLabel = canonicalCode ?? keyword ?? "Past Papers";

            var query = _context.PastPapers
                .Include(p => p.UploadedByUser)
                .Where(p => p.Status == ResourceStatus.Approved);

            if (!string.IsNullOrEmpty(canonicalCode))
            {
                query = query.Where(p =>
                    p.CourseCode.Contains(canonicalCode) ||
                    (!string.IsNullOrEmpty(compactCode) && p.CourseCode.Contains(compactCode)) ||
                    (!string.IsNullOrEmpty(deptLetters) && !string.IsNullOrEmpty(codeNumber) && p.CourseCode.Contains(deptLetters) && p.CourseCode.Contains(codeNumber)) ||
                    p.Title.Contains(canonicalCode) ||
                    p.SubjectName.Contains(canonicalCode));
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p =>
                    p.Title.Contains(keyword) ||
                    p.SubjectName.Contains(keyword) ||
                    p.CourseCode.Contains(keyword) ||
                    (p.Description != null && p.Description.Contains(keyword)) ||
                    p.ProfessorName.Contains(keyword));
            }

            var papers = await query
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.CreatedDate)
                .ToListAsync();

            if (papers.Count > 0)
            {
                response.TotalMatches = papers.Count;
                foreach (var p in papers)
                {
                    response.Resources.Add(new AiResourceItemDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        SubjectName = p.SubjectName,
                        CourseCode = p.CourseCode,
                        ResourceType = "Past Paper",
                        Department = p.Department.ToString(),
                        Year = ((int)p.Year).ToString(),
                        Semester = p.Semester.ToString(),
                        ProfessorName = p.ProfessorName,
                        DetailsUrl = $"/Resource/PastPaperDetails/{p.Id}",
                        DownloadUrl = $"/Resource/Download/{p.Id}?type=paper",
                        DownloadCount = p.DownloadCount,
                        ViewCount = p.ViewCount
                    });
                }

                var sb = new StringBuilder();
                sb.Append($"<p>I found <strong>{papers.Count} approved past paper{(papers.Count > 1 ? "s" : "")}</strong> for <strong>{targetLabel}</strong> in the database:</p>");
                sb.Append("<ul style=\"margin: 0.5rem 0 0.75rem 1.25rem; padding: 0;\">");
                foreach (var p in papers)
                {
                    var prof = string.IsNullOrWhiteSpace(p.ProfessorName) ? "" : $" • Prof. {p.ProfessorName}";
                    sb.Append("<li style=\"margin-bottom: 0.6rem;\">");
                    sb.Append($"<strong><a href=\"/Resource/PastPaperDetails/{p.Id}\">{p.Title}</a></strong> ");
                    sb.Append($"<span style=\"color: #9ca3af;\">({p.CourseCode} • {p.Semester} {(int)p.Year}{prof})</span><br/>");
                    sb.Append($"<small style=\"color: #6b7280;\">Subject: {p.SubjectName} | Downloads: {p.DownloadCount} | Views: {p.ViewCount}</small><br/>");
                    sb.Append($"<a href=\"/Resource/PastPaperDetails/{p.Id}\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">View Details &rarr;</a> &nbsp;|&nbsp; ");
                    sb.Append($"<a href=\"/Resource/Download/{p.Id}?type=paper\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">Download File &darr;</a>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append($"<p style=\"margin-top: 0.5rem;\">You can also search all past exams in the <a href=\"/Resource/PastPapers?search={Uri.EscapeDataString(targetLabel)}\">Past Papers catalog</a>.</p>");
                response.Reply = sb.ToString();
            }
            else
            {
                // Check if any pending papers exist or if notes/lectures exist
                int pendingCount = 0;
                if (!string.IsNullOrEmpty(canonicalCode))
                {
                    pendingCount = await _context.PastPapers.CountAsync(p =>
                        (p.CourseCode.Contains(canonicalCode) ||
                         (!string.IsNullOrEmpty(compactCode) && p.CourseCode.Contains(compactCode)) ||
                         (!string.IsNullOrEmpty(deptLetters) && !string.IsNullOrEmpty(codeNumber) && p.CourseCode.Contains(deptLetters) && p.CourseCode.Contains(codeNumber)) ||
                         p.Title.Contains(canonicalCode) ||
                         p.SubjectName.Contains(canonicalCode)) &&
                        p.Status == ResourceStatus.Pending);
                }

                var sb = new StringBuilder();
                if (pendingCount > 0)
                {
                    sb.Append($"<p>I searched the database through <code>ApplicationDbContext</code> for <strong>{targetLabel}</strong> past papers. There {(pendingCount == 1 ? "is 1 past paper" : $"are {pendingCount} past papers")} currently awaiting admin approval, but no approved papers are published yet.</p>");
                }
                else
                {
                    sb.Append($"<p>I searched the database through <code>ApplicationDbContext</code> for <strong>{targetLabel}</strong> past papers, but no matching approved records were found.</p>");
                }

                // Suggest existing notes or lectures if any exist for this course
                var relatedNotes = await _context.Notes
                    .Where(n => n.Status == ResourceStatus.Approved && !string.IsNullOrEmpty(canonicalCode) && (
                        n.CourseCode.Contains(canonicalCode) ||
                        (!string.IsNullOrEmpty(compactCode) && n.CourseCode.Contains(compactCode)) ||
                        n.SubjectName.Contains(canonicalCode)))
                    .Take(3)
                    .ToListAsync();

                if (relatedNotes.Count > 0)
                {
                    sb.Append($"<p>However, related study notes are available for <strong>{targetLabel}</strong>:</p><ul>");
                    foreach (var n in relatedNotes)
                    {
                        sb.Append($"<li><a href=\"/Resource/NoteDetails/{n.Id}\"><strong>{n.Title}</strong></a> ({n.SubjectName})</li>");
                    }
                    sb.Append("</ul>");
                }

                sb.Append($"<p>You can check the <a href=\"/Resource/PastPapers\">Past Papers page</a> or ask your CR/Admin to upload materials.</p>");
                response.Reply = sb.ToString();
            }
        }

        private async Task SearchNotesAsync(
            AiQueryResponse response,
            string? canonicalCode,
            string? compactCode,
            string? deptLetters,
            string? codeNumber,
            string? keyword)
        {
            var targetLabel = canonicalCode ?? keyword ?? "Study Notes";

            var query = _context.Notes
                .Include(n => n.UploadedByUser)
                .Where(n => n.Status == ResourceStatus.Approved);

            if (!string.IsNullOrEmpty(canonicalCode))
            {
                query = query.Where(n =>
                    n.CourseCode.Contains(canonicalCode) ||
                    (!string.IsNullOrEmpty(compactCode) && n.CourseCode.Contains(compactCode)) ||
                    (!string.IsNullOrEmpty(deptLetters) && !string.IsNullOrEmpty(codeNumber) && n.CourseCode.Contains(deptLetters) && n.CourseCode.Contains(codeNumber)) ||
                    n.Title.Contains(canonicalCode) ||
                    n.SubjectName.Contains(canonicalCode));
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(n =>
                    n.Title.Contains(keyword) ||
                    n.SubjectName.Contains(keyword) ||
                    n.CourseCode.Contains(keyword) ||
                    (n.Description != null && n.Description.Contains(keyword)) ||
                    (n.ProfessorName != null && n.ProfessorName.Contains(keyword)));
            }

            var notes = await query
                .OrderByDescending(n => n.AverageRating)
                .ThenByDescending(n => n.CreatedDate)
                .ToListAsync();

            if (notes.Count > 0)
            {
                response.TotalMatches = notes.Count;
                foreach (var n in notes)
                {
                    response.Resources.Add(new AiResourceItemDto
                    {
                        Id = n.Id,
                        Title = n.Title,
                        SubjectName = n.SubjectName,
                        CourseCode = n.CourseCode,
                        ResourceType = "Note",
                        Department = n.Department.ToString(),
                        Year = ((int)n.Year).ToString(),
                        Semester = n.Semester.ToString(),
                        ProfessorName = n.ProfessorName ?? string.Empty,
                        DetailsUrl = $"/Resource/NoteDetails/{n.Id}",
                        DownloadUrl = $"/Resource/Download/{n.Id}?type=note",
                        DownloadCount = n.DownloadCount,
                        ViewCount = n.ViewCount,
                        AverageRating = n.AverageRating
                    });
                }

                var sb = new StringBuilder();
                sb.Append($"<p>I found <strong>{notes.Count} approved study note{(notes.Count > 1 ? "s" : "")}</strong> for <strong>{targetLabel}</strong> in the database:</p>");
                sb.Append("<ul style=\"margin: 0.5rem 0 0.75rem 1.25rem; padding: 0;\">");
                foreach (var n in notes)
                {
                    var ratingStr = n.TotalRatings > 0 ? $" • ★ {n.AverageRating:F1} ({n.TotalRatings})" : "";
                    sb.Append("<li style=\"margin-bottom: 0.6rem;\">");
                    sb.Append($"<strong><a href=\"/Resource/NoteDetails/{n.Id}\">{n.Title}</a></strong> ");
                    sb.Append($"<span style=\"color: #9ca3af;\">({n.CourseCode}{ratingStr})</span><br/>");
                    sb.Append($"<small style=\"color: #6b7280;\">Subject: {n.SubjectName} | Downloads: {n.DownloadCount}</small><br/>");
                    sb.Append($"<a href=\"/Resource/NoteDetails/{n.Id}\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">View Details &rarr;</a> &nbsp;|&nbsp; ");
                    sb.Append($"<a href=\"/Resource/Download/{n.Id}?type=note\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">Download Note &darr;</a>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                response.Reply = sb.ToString();
            }
            else
            {
                response.Reply = $"<p>I searched the database through <code>ApplicationDbContext</code> for <strong>{targetLabel}</strong> notes, but no approved student notes are currently available. Check the <a href=\"/Resource/Notes\">Notes Section</a> or upload one to help your peers.</p>";
            }
        }

        private async Task SearchLecturesAsync(
            AiQueryResponse response,
            string? canonicalCode,
            string? compactCode,
            string? deptLetters,
            string? codeNumber,
            string? keyword)
        {
            var targetLabel = canonicalCode ?? keyword ?? "Lectures";

            var query = _context.Lectures
                .Include(l => l.UploadedByUser)
                .Where(l => l.Status == ResourceStatus.Approved);

            if (!string.IsNullOrEmpty(canonicalCode))
            {
                query = query.Where(l =>
                    l.CourseCode.Contains(canonicalCode) ||
                    (!string.IsNullOrEmpty(compactCode) && l.CourseCode.Contains(compactCode)) ||
                    (!string.IsNullOrEmpty(deptLetters) && !string.IsNullOrEmpty(codeNumber) && l.CourseCode.Contains(deptLetters) && l.CourseCode.Contains(codeNumber)) ||
                    l.Title.Contains(canonicalCode) ||
                    l.SubjectName.Contains(canonicalCode));
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(l =>
                    l.Title.Contains(keyword) ||
                    l.SubjectName.Contains(keyword) ||
                    l.CourseCode.Contains(keyword) ||
                    (l.Description != null && l.Description.Contains(keyword)) ||
                    (l.ProfessorName != null && l.ProfessorName.Contains(keyword)));
            }

            var lectures = await query
                .OrderByDescending(l => l.CreatedDate)
                .ToListAsync();

            if (lectures.Count > 0)
            {
                response.TotalMatches = lectures.Count;
                foreach (var l in lectures)
                {
                    response.Resources.Add(new AiResourceItemDto
                    {
                        Id = l.Id,
                        Title = l.Title,
                        SubjectName = l.SubjectName,
                        CourseCode = l.CourseCode,
                        ResourceType = "Lecture",
                        Department = l.Department.ToString(),
                        Year = ((int)l.Year).ToString(),
                        Semester = l.Semester.ToString(),
                        ProfessorName = l.ProfessorName ?? string.Empty,
                        DetailsUrl = $"/Resource/LectureDetails/{l.Id}",
                        DownloadUrl = $"/Resource/Download/{l.Id}?type=lecture",
                        DownloadCount = l.DownloadCount,
                        ViewCount = l.ViewCount
                    });
                }

                var sb = new StringBuilder();
                sb.Append($"<p>I found <strong>{lectures.Count} lecture slide set{(lectures.Count > 1 ? "s" : "")}</strong> for <strong>{targetLabel}</strong> in the database:</p>");
                sb.Append("<ul style=\"margin: 0.5rem 0 0.75rem 1.25rem; padding: 0;\">");
                foreach (var l in lectures)
                {
                    sb.Append("<li style=\"margin-bottom: 0.6rem;\">");
                    sb.Append($"<strong><a href=\"/Resource/LectureDetails/{l.Id}\">{l.Title}</a></strong> ");
                    sb.Append($"<span style=\"color: #9ca3af;\">({l.CourseCode} • {l.Semester} {(int)l.Year})</span><br/>");
                    sb.Append($"<small style=\"color: #6b7280;\">Subject: {l.SubjectName} | Downloads: {l.DownloadCount}</small><br/>");
                    sb.Append($"<a href=\"/Resource/LectureDetails/{l.Id}\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">View Details &rarr;</a> &nbsp;|&nbsp; ");
                    sb.Append($"<a href=\"/Resource/Download/{l.Id}?type=lecture\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">Download Slides &darr;</a>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                response.Reply = sb.ToString();
            }
            else
            {
                response.Reply = $"<p>I searched the database through <code>ApplicationDbContext</code> for <strong>{targetLabel}</strong> lecture materials, but no approved lectures were found. You can browse the <a href=\"/Resource/Lectures\">Lectures Page</a> to explore other subjects.</p>";
            }
        }

        private async Task SearchAllResourcesAsync(
            AiQueryResponse response,
            string? canonicalCode,
            string? compactCode,
            string? deptLetters,
            string? codeNumber,
            string? keyword)
        {
            var targetLabel = canonicalCode ?? keyword ?? "Resources";

            // Search Past Papers
            var papersQuery = _context.PastPapers
                .Include(p => p.UploadedByUser)
                .Where(p => p.Status == ResourceStatus.Approved);

            // Search Notes
            var notesQuery = _context.Notes
                .Include(n => n.UploadedByUser)
                .Where(n => n.Status == ResourceStatus.Approved);

            // Search Lectures
            var lecturesQuery = _context.Lectures
                .Include(l => l.UploadedByUser)
                .Where(l => l.Status == ResourceStatus.Approved);

            if (!string.IsNullOrEmpty(canonicalCode))
            {
                papersQuery = papersQuery.Where(p =>
                    p.CourseCode.Contains(canonicalCode) ||
                    (!string.IsNullOrEmpty(compactCode) && p.CourseCode.Contains(compactCode)) ||
                    (!string.IsNullOrEmpty(deptLetters) && !string.IsNullOrEmpty(codeNumber) && p.CourseCode.Contains(deptLetters) && p.CourseCode.Contains(codeNumber)) ||
                    p.Title.Contains(canonicalCode) ||
                    p.SubjectName.Contains(canonicalCode));

                notesQuery = notesQuery.Where(n =>
                    n.CourseCode.Contains(canonicalCode) ||
                    (!string.IsNullOrEmpty(compactCode) && n.CourseCode.Contains(compactCode)) ||
                    (!string.IsNullOrEmpty(deptLetters) && !string.IsNullOrEmpty(codeNumber) && n.CourseCode.Contains(deptLetters) && n.CourseCode.Contains(codeNumber)) ||
                    n.Title.Contains(canonicalCode) ||
                    n.SubjectName.Contains(canonicalCode));

                lecturesQuery = lecturesQuery.Where(l =>
                    l.CourseCode.Contains(canonicalCode) ||
                    (!string.IsNullOrEmpty(compactCode) && l.CourseCode.Contains(compactCode)) ||
                    (!string.IsNullOrEmpty(deptLetters) && !string.IsNullOrEmpty(codeNumber) && l.CourseCode.Contains(deptLetters) && l.CourseCode.Contains(codeNumber)) ||
                    l.Title.Contains(canonicalCode) ||
                    l.SubjectName.Contains(canonicalCode));
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                papersQuery = papersQuery.Where(p =>
                    p.Title.Contains(keyword) || p.SubjectName.Contains(keyword) || p.CourseCode.Contains(keyword));
                notesQuery = notesQuery.Where(n =>
                    n.Title.Contains(keyword) || n.SubjectName.Contains(keyword) || n.CourseCode.Contains(keyword));
                lecturesQuery = lecturesQuery.Where(l =>
                    l.Title.Contains(keyword) || l.SubjectName.Contains(keyword) || l.CourseCode.Contains(keyword));
            }

            var papers = await papersQuery.OrderByDescending(p => p.Year).Take(5).ToListAsync();
            var notes = await notesQuery.OrderByDescending(n => n.AverageRating).Take(5).ToListAsync();
            var lectures = await lecturesQuery.OrderByDescending(l => l.CreatedDate).Take(5).ToListAsync();

            int total = papers.Count + notes.Count + lectures.Count;
            response.TotalMatches = total;

            foreach (var p in papers)
            {
                response.Resources.Add(new AiResourceItemDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    SubjectName = p.SubjectName,
                    CourseCode = p.CourseCode,
                    ResourceType = "Past Paper",
                    DetailsUrl = $"/Resource/PastPaperDetails/{p.Id}",
                    DownloadUrl = $"/Resource/Download/{p.Id}?type=paper",
                    DownloadCount = p.DownloadCount,
                    ViewCount = p.ViewCount
                });
            }

            foreach (var n in notes)
            {
                response.Resources.Add(new AiResourceItemDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    SubjectName = n.SubjectName,
                    CourseCode = n.CourseCode,
                    ResourceType = "Note",
                    DetailsUrl = $"/Resource/NoteDetails/{n.Id}",
                    DownloadUrl = $"/Resource/Download/{n.Id}?type=note",
                    DownloadCount = n.DownloadCount,
                    ViewCount = n.ViewCount,
                    AverageRating = n.AverageRating
                });
            }

            foreach (var l in lectures)
            {
                response.Resources.Add(new AiResourceItemDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    SubjectName = l.SubjectName,
                    CourseCode = l.CourseCode,
                    ResourceType = "Lecture",
                    DetailsUrl = $"/Resource/LectureDetails/{l.Id}",
                    DownloadUrl = $"/Resource/Download/{l.Id}?type=lecture",
                    DownloadCount = l.DownloadCount,
                    ViewCount = l.ViewCount
                });
            }

            if (total > 0)
            {
                var sb = new StringBuilder();
                sb.Append($"<p>I found <strong>{total} resource{(total > 1 ? "s" : "")}</strong> matching <strong>{targetLabel}</strong> in the database:</p>");
                sb.Append("<ul style=\"margin: 0.5rem 0 0.75rem 1.25rem; padding: 0;\">");
                foreach (var p in papers)
                {
                    sb.Append($"<li>📄 <strong>Past Paper:</strong> <a href=\"/Resource/PastPaperDetails/{p.Id}\">{p.Title}</a> ({p.CourseCode})</li>");
                }
                foreach (var n in notes)
                {
                    sb.Append($"<li>📝 <strong>Note:</strong> <a href=\"/Resource/NoteDetails/{n.Id}\">{n.Title}</a> ({n.CourseCode})</li>");
                }
                foreach (var l in lectures)
                {
                    sb.Append($"<li>🎥 <strong>Lecture:</strong> <a href=\"/Resource/LectureDetails/{l.Id}\">{l.Title}</a> ({l.CourseCode})</li>");
                }
                sb.Append("</ul>");
                response.Reply = sb.ToString();
            }
            else
            {
                response.Reply = $"<p>I searched the database through <code>ApplicationDbContext</code> for <strong>{targetLabel}</strong>, but no matching approved resources were found. You can browse all materials in the <a href=\"/Resource/PastPapers\">Past Papers</a>, <a href=\"/Resource/Notes\">Notes</a>, or <a href=\"/StarterKit\">Starter Kit</a> sections.</p>";
            }
        }

        private static bool IsStarterKitQuery(string query)
        {
            return ContainsAny(query, "starter kit", "starterkit", "first-year starter", "first year starter", "freshman kit");
        }

        private static bool IsStudyTipsQuery(string query)
        {
            return ContainsAny(query, "study tip", "study tips", "exam tip", "exam tips", "midterm prep", "preparation tips", "how to study", "exam preparation");
        }

        private static string BuildStarterKitReply()
        {
            return "<p>The <strong>First-Year Starter Kit</strong> is designed to fast-track your semester preparation! Here is how to use it:</p>" +
                   "<ul>" +
                   "<li><strong>Department &amp; Semester Filter:</strong> Select your engineering or science department and your current intake semester.</li>" +
                   "<li><strong>Subject Bundles:</strong> Materials are automatically grouped into core subjects (e.g., Structured Programming, Calculus, English).</li>" +
                   "<li><strong>All-in-One Access:</strong> Each subject group bundles approved past exam papers, lecture slide decks, and top-rated student notes.</li>" +
                   "</ul>" +
                   "<p>Ready to get started? <a href=\"/StarterKit\" style=\"font-weight: 700; text-decoration: underline;\">Launch the First-Year Starter Kit &rarr;</a></p>";
        }

        private static string BuildStudyTipsReply()
        {
            return "<p>Here are <strong>5 proven study strategies</strong> to excel in your upcoming university exams:</p>" +
                   "<ol style=\"margin: 0.5rem 0 0.75rem 1.25rem; padding: 0;\">" +
                   "<li style=\"margin-bottom: 0.4rem;\"><strong>Active Recall with Past Papers:</strong> Solve at least 3 previous semester exam papers under timed conditions instead of passive re-reading.</li>" +
                   "<li style=\"margin-bottom: 0.4rem;\"><strong>Spaced Repetition:</strong> Review difficult formulas and programming concepts across multiple 45-minute study intervals.</li>" +
                   "<li style=\"margin-bottom: 0.4rem;\"><strong>The Feynman Technique:</strong> Explain complex algorithms or proofs out loud in plain, simple language without jargon.</li>" +
                   "<li style=\"margin-bottom: 0.4rem;\"><strong>Focus on Weak Areas First:</strong> Identify the topics you missed on quizzes or lab evaluations and tackle them with lecture notes.</li>" +
                   "<li style=\"margin-bottom: 0.4rem;\"><strong>Rest &amp; Sleep Cycle:</strong> Avoid all-nighters before exam day; proper sleep consolidates your neural problem-solving pathways.</li>" +
                   "</ol>" +
                   "<p>Tip: Search for your course code here in StudyHub AI to download the relevant past papers for practice!</p>";
        }

        private static string? ExtractKeyword(string query, string? canonicalCode, string? compactCode)
        {
            var cleaned = query;
            if (!string.IsNullOrEmpty(canonicalCode))
            {
                cleaned = Regex.Replace(cleaned, Regex.Escape(canonicalCode), " ", RegexOptions.IgnoreCase);
            }
            if (!string.IsNullOrEmpty(compactCode))
            {
                cleaned = Regex.Replace(cleaned, Regex.Escape(compactCode), " ", RegexOptions.IgnoreCase);
            }

            cleaned = Regex.Replace(cleaned, @"[^\w\s]", " ");
            var words = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(w => !StopWords.Contains(w) && w.Length > 2)
                .ToList();

            return words.Count > 0 ? string.Join(" ", words) : null;
        }

        private static bool ContainsAny(string text, params string[] values)
        {
            foreach (var val in values)
            {
                if (text.Contains(val, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
