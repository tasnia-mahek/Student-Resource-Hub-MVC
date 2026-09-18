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

        private static readonly HashSet<string> FillerWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "a", "about", "all", "am", "an", "and", "any", "anything", "are", "as", "at",
            "available", "be", "been", "being", "bring", "by", "can", "check", "could",
            "did", "display", "do", "does", "fetch", "find", "for", "from", "get", "give",
            "got", "had", "has", "have", "hello", "help", "hey", "hi", "how", "i", "in",
            "into", "is", "it", "its", "just", "kindly", "look", "looking", "me", "my",
            "need", "of", "on", "onto", "our", "ours", "over", "please", "provide",
            "search", "searching", "see", "show", "some", "something", "tell", "the",
            "their", "there", "to", "under", "us", "want", "was", "we", "were", "what",
            "where", "which", "who", "whom", "will", "with", "would", "you", "your", "yours",
            // Generic resource labels that are not subject names
            "material", "materials", "study", "studies", "studying", "resource", "resources",
            "content", "contents", "item", "items", "file", "files", "document", "documents",
            "course", "courses", "subject", "subjects", "topic", "topics", "class", "classes"
        };

        private static readonly HashSet<string> ResourceIndicatorWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "paper", "papers", "pastpaper", "pastpapers", "question", "questions",
            "past", "previous", "prior", "old", "solution", "solutions", "solve", "solved",
            "note", "notes", "handout", "handouts",
            "lecture", "lectures", "slide", "slides", "recording", "recordings",
            "video", "videos", "presentation", "presentations", "deck", "decks"
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
                    Reply = "<p>Please enter a question, topic, course code, or resource request so I can search our database for you.</p>",
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

            // 3. Parse user intent and extract all entities
            var intent = ParseQueryIntent(query);

            // 4. Route search based on extracted intent
            if (intent.WantsPastPapers && !intent.WantsNotes && !intent.WantsLectures && !intent.IsGeneralStudyMaterial)
            {
                await SearchPastPapersAsync(response, intent);
            }
            else if (intent.WantsNotes && !intent.WantsPastPapers && !intent.WantsLectures && !intent.IsGeneralStudyMaterial)
            {
                await SearchNotesAsync(response, intent);
            }
            else if (intent.WantsLectures && !intent.WantsPastPapers && !intent.WantsNotes && !intent.IsGeneralStudyMaterial)
            {
                await SearchLecturesAsync(response, intent);
            }
            else
            {
                // Unspecified resource type, general materials, or multi-resource query: search across all resources!
                await SearchAllResourcesAsync(response, intent);
            }

            return response;
        }

        #region Natural Language Intent & Entity Parser

        private class QueryIntent
        {
            public string RawQuery { get; set; } = string.Empty;
            public string? CanonicalCourseCode { get; set; }
            public string? CompactCourseCode { get; set; }
            public string? DeptLetters { get; set; }
            public string? CourseNumber { get; set; }
            public int? Year { get; set; }
            public Semester? Semester { get; set; }
            public string? ExamType { get; set; } // "final", "midterm", "quiz"
            public bool WantsPastPapers { get; set; }
            public bool WantsNotes { get; set; }
            public bool WantsLectures { get; set; }
            public bool IsGeneralStudyMaterial { get; set; }
            public string? TopicOrSubject { get; set; }
            public string? TopicStem { get; set; }
            public List<string> TopicWords { get; set; } = new();

            public string GetDisplayTarget()
            {
                var parts = new List<string>();
                if (!string.IsNullOrEmpty(CanonicalCourseCode))
                {
                    parts.Add(CanonicalCourseCode);
                }
                if (!string.IsNullOrEmpty(TopicOrSubject))
                {
                    parts.Add(TopicOrSubject);
                }
                if (Semester.HasValue)
                {
                    parts.Add(Semester.Value.ToString());
                }
                if (Year.HasValue)
                {
                    parts.Add(Year.Value.ToString());
                }
                if (!string.IsNullOrEmpty(ExamType))
                {
                    parts.Add(char.ToUpper(ExamType[0]) + ExamType[1..]);
                }

                return parts.Count > 0 ? string.Join(" ", parts) : "Requested Materials";
            }
        }

        private static QueryIntent ParseQueryIntent(string query)
        {
            var intent = new QueryIntent
            {
                RawQuery = query
            };

            // A. Extract Year (e.g. 2021, 2022, 2023, 2024, 2025, 2026)
            var yearMatch = Regex.Match(query, @"\b(20[12]\d)\b");
            if (yearMatch.Success && int.TryParse(yearMatch.Groups[1].Value, out var parsedYear))
            {
                intent.Year = parsedYear;
            }

            // B. Extract Semester (Fall, Spring, Summer)
            var semMatch = Regex.Match(query, @"\b(fall|spring|summer)\b", RegexOptions.IgnoreCase);
            if (semMatch.Success && Enum.TryParse<Semester>(semMatch.Groups[1].Value, true, out var parsedSem))
            {
                intent.Semester = parsedSem;
            }

            // C. Extract Exam Type (Final, Midterm, Quiz)
            var examMatch = Regex.Match(query, @"\b(final|finals|midterm|midterms|mid-term|quiz|quizzes|terminal)\b", RegexOptions.IgnoreCase);
            if (examMatch.Success)
            {
                var val = examMatch.Groups[1].Value.ToLowerInvariant();
                if (val.StartsWith("final")) intent.ExamType = "final";
                else if (val.StartsWith("mid")) intent.ExamType = "midterm";
                else if (val.StartsWith("quiz")) intent.ExamType = "quiz";
                else intent.ExamType = val;
            }

            // D. Extract Course Code (e.g. CSE 110, CSE-110, CSE110, MAT 101, EEE 201)
            var courseMatches = Regex.Matches(query, @"\b([A-Za-z]{2,5})\s*[-_]?\s*(\d{3,4}[A-Za-z]?)\b");
            foreach (Match cm in courseMatches)
            {
                var dept = cm.Groups[1].Value.ToUpperInvariant();
                var num = cm.Groups[2].Value.ToUpperInvariant();
                if (!FillerWords.Contains(dept) && !ResourceIndicatorWords.Contains(dept))
                {
                    intent.DeptLetters = dept;
                    intent.CourseNumber = num;
                    intent.CanonicalCourseCode = $"{dept} {num}";
                    intent.CompactCourseCode = $"{dept}{num}";
                    break;
                }
            }

            if (string.IsNullOrEmpty(intent.CanonicalCourseCode))
            {
                // Fallback: standalone 3-digit course number (avoiding 4-digit years)
                var numOnlyMatch = Regex.Match(query, @"\b(\d{3})\b");
                if (numOnlyMatch.Success && (!intent.Year.HasValue || intent.Year.Value.ToString() != numOnlyMatch.Value))
                {
                    intent.CourseNumber = numOnlyMatch.Value;
                    intent.CanonicalCourseCode = numOnlyMatch.Value;
                    intent.CompactCourseCode = numOnlyMatch.Value;
                }
            }

            // E. Detect Resource Type Preferences
            bool hasGeneralKeywords = ContainsAny(query, "study material", "study materials", "materials", "anything", "all", "everything", "resource", "resources");
            intent.IsGeneralStudyMaterial = hasGeneralKeywords;

            bool mentionsPastPapers = ContainsAny(query,
                "past paper", "past papers", "pastpaper", "pastpapers",
                "question paper", "question papers", "previous paper", "previous papers",
                "exam paper", "exam papers", "paper", "papers", "question", "questions");

            bool mentionsNotes = ContainsAny(query,
                "note", "notes", "handout", "handouts", "lecture note", "lecture notes",
                "study note", "study notes", "cheat sheet", "cheatsheet", "summary", "summaries");

            bool mentionsLectures = ContainsAny(query,
                "lecture", "lectures", "slide", "slides", "recording", "recordings",
                "video", "videos", "presentation", "presentations", "deck", "decks",
                "class recording", "class video");

            // Exam type indicator (e.g., "final", "midterm") without notes/lectures strongly indicates past papers
            bool examTypeWithoutOtherTypes = !string.IsNullOrEmpty(intent.ExamType) && !mentionsNotes && !mentionsLectures;

            intent.WantsPastPapers = mentionsPastPapers || examTypeWithoutOtherTypes;
            intent.WantsNotes = mentionsNotes;
            intent.WantsLectures = mentionsLectures;

            // F. Extract Subject / Topic Keywords
            // Strip out course codes, year, semester, and exam type from the query string
            var workingQuery = query;

            if (!string.IsNullOrEmpty(intent.CanonicalCourseCode))
            {
                workingQuery = Regex.Replace(workingQuery, Regex.Escape(intent.CanonicalCourseCode), " ", RegexOptions.IgnoreCase);
            }
            if (!string.IsNullOrEmpty(intent.CompactCourseCode))
            {
                workingQuery = Regex.Replace(workingQuery, Regex.Escape(intent.CompactCourseCode), " ", RegexOptions.IgnoreCase);
            }
            if (!string.IsNullOrEmpty(intent.DeptLetters))
            {
                workingQuery = Regex.Replace(workingQuery, $@"\b{Regex.Escape(intent.DeptLetters)}\b", " ", RegexOptions.IgnoreCase);
            }
            if (!string.IsNullOrEmpty(intent.CourseNumber))
            {
                workingQuery = Regex.Replace(workingQuery, $@"\b{Regex.Escape(intent.CourseNumber)}\b", " ", RegexOptions.IgnoreCase);
            }
            if (intent.Year.HasValue)
            {
                workingQuery = Regex.Replace(workingQuery, $@"\b{intent.Year.Value}\b", " ");
            }
            if (intent.Semester.HasValue)
            {
                workingQuery = Regex.Replace(workingQuery, $@"\b{intent.Semester.Value}\b", " ", RegexOptions.IgnoreCase);
            }
            if (!string.IsNullOrEmpty(intent.ExamType))
            {
                workingQuery = Regex.Replace(workingQuery, @"\b(final|finals|midterm|midterms|mid-term|quiz|quizzes|terminal)\b", " ", RegexOptions.IgnoreCase);
            }

            // Remove punctuation and clean whitespace
            workingQuery = Regex.Replace(workingQuery, @"[^\w\s]", " ");

            var tokens = workingQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(w => !FillerWords.Contains(w) && !ResourceIndicatorWords.Contains(w) && w.Length > 1)
                .ToList();

            if (tokens.Count > 0)
            {
                intent.TopicOrSubject = string.Join(" ", tokens);
                intent.TopicWords = tokens.Select(t => t.ToLowerInvariant()).ToList();

                // Compute stem word for single or primary token (e.g. "networking" -> "network")
                if (tokens.Count == 1)
                {
                    intent.TopicStem = GetStemWord(tokens[0]);
                }
            }

            return intent;
        }

        private static string? GetStemWord(string word)
        {
            if (word.EndsWith("ing", StringComparison.OrdinalIgnoreCase) && word.Length > 5)
            {
                return word[..^3];
            }
            if (word.EndsWith("s", StringComparison.OrdinalIgnoreCase) && !word.EndsWith("ss", StringComparison.OrdinalIgnoreCase) && word.Length > 3)
            {
                return word[..^1];
            }
            return null;
        }

        #endregion

        #region Search Implementations

        private async Task SearchPastPapersAsync(AiQueryResponse response, QueryIntent intent)
        {
            var targetLabel = intent.GetDisplayTarget();

            var query = _context.PastPapers
                .Include(p => p.UploadedByUser)
                .Where(p => p.Status == ResourceStatus.Approved);

            // Filter by Course Code
            if (!string.IsNullOrEmpty(intent.CanonicalCourseCode))
            {
                var canonical = intent.CanonicalCourseCode;
                var compact = intent.CompactCourseCode;
                var dept = intent.DeptLetters;
                var num = intent.CourseNumber;

                query = query.Where(p =>
                    p.CourseCode.Contains(canonical) ||
                    (!string.IsNullOrEmpty(compact) && p.CourseCode.Contains(compact)) ||
                    (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(num) && p.CourseCode.Contains(dept) && p.CourseCode.Contains(num)) ||
                    p.Title.Contains(canonical) ||
                    p.SubjectName.Contains(canonical));
            }

            // Filter by Year
            if (intent.Year.HasValue)
            {
                query = query.Where(p => (int)p.Year == intent.Year.Value);
            }

            // Filter by Semester
            if (intent.Semester.HasValue)
            {
                query = query.Where(p => p.Semester == intent.Semester.Value);
            }

            // Filter by Exam Type (e.g. "final", "midterm")
            if (!string.IsNullOrEmpty(intent.ExamType))
            {
                var examType = intent.ExamType;
                query = query.Where(p =>
                    p.Title.Contains(examType) ||
                    (p.Description != null && p.Description.Contains(examType)));
            }

            // Filter by Topic / Subject
            if (!string.IsNullOrEmpty(intent.TopicOrSubject))
            {
                var topic = intent.TopicOrSubject;
                var stem = intent.TopicStem;

                if (intent.TopicWords.Count >= 2)
                {
                    var w0 = intent.TopicWords[0];
                    var w1 = intent.TopicWords[1];

                    query = query.Where(p =>
                        p.SubjectName.Contains(topic) ||
                        p.Title.Contains(topic) ||
                        p.CourseCode.Contains(topic) ||
                        (p.Description != null && p.Description.Contains(topic)) ||
                        p.ProfessorName.Contains(topic) ||
                        ((p.SubjectName.Contains(w0) || p.Title.Contains(w0)) && (p.SubjectName.Contains(w1) || p.Title.Contains(w1))));
                }
                else
                {
                    query = query.Where(p =>
                        p.SubjectName.Contains(topic) ||
                        p.Title.Contains(topic) ||
                        p.CourseCode.Contains(topic) ||
                        (p.Description != null && p.Description.Contains(topic)) ||
                        p.ProfessorName.Contains(topic) ||
                        (!string.IsNullOrEmpty(stem) && (p.SubjectName.Contains(stem) || p.Title.Contains(stem) || (p.Description != null && p.Description.Contains(stem)))));
                }
            }

            var papers = await query
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.CreatedDate)
                .ToListAsync();

            // Graceful fallback: If strict filter returned 0 results, try relaxing Year or ExamType if CourseCode is present
            List<PastPaper>? alternativePapers = null;
            if (papers.Count == 0 && !string.IsNullOrEmpty(intent.CanonicalCourseCode) && (intent.Year.HasValue || !string.IsNullOrEmpty(intent.ExamType)))
            {
                var altQuery = _context.PastPapers
                    .Include(p => p.UploadedByUser)
                    .Where(p => p.Status == ResourceStatus.Approved && (
                        p.CourseCode.Contains(intent.CanonicalCourseCode) ||
                        (!string.IsNullOrEmpty(intent.CompactCourseCode) && p.CourseCode.Contains(intent.CompactCourseCode)) ||
                        p.Title.Contains(intent.CanonicalCourseCode) ||
                        p.SubjectName.Contains(intent.CanonicalCourseCode)));

                alternativePapers = await altQuery
                    .OrderByDescending(p => p.Year)
                    .ThenByDescending(p => p.CreatedDate)
                    .Take(5)
                    .ToListAsync();
            }

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
                    sb.Append($"📄 <strong><a href=\"/Resource/PastPaperDetails/{p.Id}\">{p.Title}</a></strong> ");
                    sb.Append($"<span style=\"color: #9ca3af;\">({p.CourseCode} • {p.Semester} {(int)p.Year}{prof})</span><br/>");
                    sb.Append($"<small style=\"color: #6b7280;\">Subject: {p.SubjectName} | Downloads: {p.DownloadCount} | Views: {p.ViewCount}</small><br/>");
                    sb.Append($"<a href=\"/Resource/PastPaperDetails/{p.Id}\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">View Details &rarr;</a> &nbsp;|&nbsp; ");
                    sb.Append($"<a href=\"/Resource/Download/{p.Id}?type=paper\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">Download File &darr;</a>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append($"<p style=\"margin-top: 0.5rem;\">Browse all past exams on the <a href=\"/Resource/PastPapers?search={Uri.EscapeDataString(intent.CanonicalCourseCode ?? intent.TopicOrSubject ?? string.Empty)}\">Past Papers catalog</a>.</p>");
                response.Reply = sb.ToString();
            }
            else
            {
                var sb = new StringBuilder();

                if (alternativePapers != null && alternativePapers.Count > 0)
                {
                    sb.Append($"<p>I could not find an exact match for <strong>{targetLabel}</strong>, but I found <strong>{alternativePapers.Count} other approved past paper{(alternativePapers.Count > 1 ? "s" : "")}</strong> for <strong>{intent.CanonicalCourseCode}</strong>:</p>");
                    sb.Append("<ul style=\"margin: 0.5rem 0 0.75rem 1.25rem; padding: 0;\">");
                    foreach (var p in alternativePapers)
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

                        sb.Append("<li style=\"margin-bottom: 0.6rem;\">");
                        sb.Append($"📄 <strong><a href=\"/Resource/PastPaperDetails/{p.Id}\">{p.Title}</a></strong> ");
                        sb.Append($"<span style=\"color: #9ca3af;\">({p.CourseCode} • {p.Semester} {(int)p.Year})</span><br/>");
                        sb.Append($"<a href=\"/Resource/PastPaperDetails/{p.Id}\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">View Details &rarr;</a> &nbsp;|&nbsp; ");
                        sb.Append($"<a href=\"/Resource/Download/{p.Id}?type=paper\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">Download File &darr;</a>");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                    response.TotalMatches = alternativePapers.Count;
                }
                else
                {
                    sb.Append($"<p>I searched the database through <code>ApplicationDbContext</code> for <strong>{targetLabel}</strong> past papers, but no matching approved records were found.</p>");

                    // Check if related study notes exist for this subject or course code
                    var relatedNotesQuery = _context.Notes
                        .Where(n => n.Status == ResourceStatus.Approved);

                    if (!string.IsNullOrEmpty(intent.CanonicalCourseCode))
                    {
                        relatedNotesQuery = relatedNotesQuery.Where(n =>
                            n.CourseCode.Contains(intent.CanonicalCourseCode) ||
                            n.Title.Contains(intent.CanonicalCourseCode) ||
                            n.SubjectName.Contains(intent.CanonicalCourseCode));
                    }
                    else if (!string.IsNullOrEmpty(intent.TopicOrSubject))
                    {
                        relatedNotesQuery = relatedNotesQuery.Where(n =>
                            n.SubjectName.Contains(intent.TopicOrSubject) ||
                            n.Title.Contains(intent.TopicOrSubject));
                    }

                    var relatedNotes = await relatedNotesQuery.Take(3).ToListAsync();
                    if (relatedNotes.Count > 0)
                    {
                        sb.Append($"<p>However, related study notes are available:</p><ul>");
                        foreach (var n in relatedNotes)
                        {
                            sb.Append($"<li>📝 <a href=\"/Resource/NoteDetails/{n.Id}\"><strong>{n.Title}</strong></a> ({n.CourseCode} • {n.SubjectName})</li>");
                        }
                        sb.Append("</ul>");
                    }
                }

                sb.Append($"<p>You can check the <a href=\"/Resource/PastPapers\">Past Papers page</a> or request materials from your peers and CR.</p>");
                response.Reply = sb.ToString();
            }
        }

        private async Task SearchNotesAsync(AiQueryResponse response, QueryIntent intent)
        {
            var targetLabel = intent.GetDisplayTarget();

            var query = _context.Notes
                .Include(n => n.UploadedByUser)
                .Where(n => n.Status == ResourceStatus.Approved);

            // Filter by Course Code
            if (!string.IsNullOrEmpty(intent.CanonicalCourseCode))
            {
                var canonical = intent.CanonicalCourseCode;
                var compact = intent.CompactCourseCode;
                var dept = intent.DeptLetters;
                var num = intent.CourseNumber;

                query = query.Where(n =>
                    n.CourseCode.Contains(canonical) ||
                    (!string.IsNullOrEmpty(compact) && n.CourseCode.Contains(compact)) ||
                    (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(num) && n.CourseCode.Contains(dept) && n.CourseCode.Contains(num)) ||
                    n.Title.Contains(canonical) ||
                    n.SubjectName.Contains(canonical));
            }

            // Filter by Year
            if (intent.Year.HasValue)
            {
                query = query.Where(n => (int)n.Year == intent.Year.Value);
            }

            // Filter by Semester
            if (intent.Semester.HasValue)
            {
                query = query.Where(n => n.Semester == intent.Semester.Value);
            }

            // Filter by Topic / Subject
            if (!string.IsNullOrEmpty(intent.TopicOrSubject))
            {
                var topic = intent.TopicOrSubject;
                var stem = intent.TopicStem;

                if (intent.TopicWords.Count >= 2)
                {
                    var w0 = intent.TopicWords[0];
                    var w1 = intent.TopicWords[1];

                    query = query.Where(n =>
                        n.SubjectName.Contains(topic) ||
                        n.Title.Contains(topic) ||
                        n.CourseCode.Contains(topic) ||
                        (n.Description != null && n.Description.Contains(topic)) ||
                        (n.ProfessorName != null && n.ProfessorName.Contains(topic)) ||
                        ((n.SubjectName.Contains(w0) || n.Title.Contains(w0)) && (n.SubjectName.Contains(w1) || n.Title.Contains(w1))));
                }
                else
                {
                    query = query.Where(n =>
                        n.SubjectName.Contains(topic) ||
                        n.Title.Contains(topic) ||
                        n.CourseCode.Contains(topic) ||
                        (n.Description != null && n.Description.Contains(topic)) ||
                        (n.ProfessorName != null && n.ProfessorName.Contains(topic)) ||
                        (!string.IsNullOrEmpty(stem) && (n.SubjectName.Contains(stem) || n.Title.Contains(stem) || (n.Description != null && n.Description.Contains(stem)))));
                }
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
                    sb.Append($"📝 <strong><a href=\"/Resource/NoteDetails/{n.Id}\">{n.Title}</a></strong> ");
                    sb.Append($"<span style=\"color: #9ca3af;\">({n.CourseCode}{ratingStr})</span><br/>");
                    sb.Append($"<small style=\"color: #6b7280;\">Subject: {n.SubjectName} | Downloads: {n.DownloadCount}</small><br/>");
                    sb.Append($"<a href=\"/Resource/NoteDetails/{n.Id}\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">View Details &rarr;</a> &nbsp;|&nbsp; ");
                    sb.Append($"<a href=\"/Resource/Download/{n.Id}?type=note\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">Download Note &darr;</a>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append($"<p style=\"margin-top: 0.5rem;\">You can explore more student notes on the <a href=\"/Resource/Notes?search={Uri.EscapeDataString(intent.CanonicalCourseCode ?? intent.TopicOrSubject ?? string.Empty)}\">Notes catalog</a>.</p>");
                response.Reply = sb.ToString();
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append($"<p>I searched the database through <code>ApplicationDbContext</code> for <strong>{targetLabel}</strong> notes, but no approved student notes are currently available.</p>");

                // Suggest existing lectures or past papers if available
                var relatedPapers = await _context.PastPapers
                    .Where(p => p.Status == ResourceStatus.Approved && !string.IsNullOrEmpty(intent.CanonicalCourseCode) && (
                        p.CourseCode.Contains(intent.CanonicalCourseCode) ||
                        p.SubjectName.Contains(intent.CanonicalCourseCode)))
                    .Take(3)
                    .ToListAsync();

                if (relatedPapers.Count > 0)
                {
                    sb.Append($"<p>However, past exam papers are available for <strong>{intent.CanonicalCourseCode}</strong>:</p><ul>");
                    foreach (var p in relatedPapers)
                    {
                        sb.Append($"<li>📄 <a href=\"/Resource/PastPaperDetails/{p.Id}\"><strong>{p.Title}</strong></a> ({p.Semester} {(int)p.Year})</li>");
                    }
                    sb.Append("</ul>");
                }

                sb.Append($"<p>Check the <a href=\"/Resource/Notes\">Notes Section</a> or upload your own notes to help your classmates.</p>");
                response.Reply = sb.ToString();
            }
        }

        private async Task SearchLecturesAsync(AiQueryResponse response, QueryIntent intent)
        {
            var targetLabel = intent.GetDisplayTarget();

            var query = _context.Lectures
                .Include(l => l.UploadedByUser)
                .Where(l => l.Status == ResourceStatus.Approved);

            // Filter by Course Code
            if (!string.IsNullOrEmpty(intent.CanonicalCourseCode))
            {
                var canonical = intent.CanonicalCourseCode;
                var compact = intent.CompactCourseCode;
                var dept = intent.DeptLetters;
                var num = intent.CourseNumber;

                query = query.Where(l =>
                    l.CourseCode.Contains(canonical) ||
                    (!string.IsNullOrEmpty(compact) && l.CourseCode.Contains(compact)) ||
                    (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(num) && l.CourseCode.Contains(dept) && l.CourseCode.Contains(num)) ||
                    l.Title.Contains(canonical) ||
                    l.SubjectName.Contains(canonical));
            }

            // Filter by Year
            if (intent.Year.HasValue)
            {
                query = query.Where(l => (int)l.Year == intent.Year.Value);
            }

            // Filter by Semester
            if (intent.Semester.HasValue)
            {
                query = query.Where(l => l.Semester == intent.Semester.Value);
            }

            // Filter by Topic / Subject
            if (!string.IsNullOrEmpty(intent.TopicOrSubject))
            {
                var topic = intent.TopicOrSubject;
                var stem = intent.TopicStem;

                if (intent.TopicWords.Count >= 2)
                {
                    var w0 = intent.TopicWords[0];
                    var w1 = intent.TopicWords[1];

                    query = query.Where(l =>
                        l.SubjectName.Contains(topic) ||
                        l.Title.Contains(topic) ||
                        l.CourseCode.Contains(topic) ||
                        (l.Topics != null && l.Topics.Contains(topic)) ||
                        (l.Description != null && l.Description.Contains(topic)) ||
                        (l.ProfessorName != null && l.ProfessorName.Contains(topic)) ||
                        ((l.SubjectName.Contains(w0) || l.Title.Contains(w0)) && (l.SubjectName.Contains(w1) || l.Title.Contains(w1))));
                }
                else
                {
                    query = query.Where(l =>
                        l.SubjectName.Contains(topic) ||
                        l.Title.Contains(topic) ||
                        l.CourseCode.Contains(topic) ||
                        (l.Topics != null && l.Topics.Contains(topic)) ||
                        (l.Description != null && l.Description.Contains(topic)) ||
                        (l.ProfessorName != null && l.ProfessorName.Contains(topic)) ||
                        (!string.IsNullOrEmpty(stem) && (l.SubjectName.Contains(stem) || l.Title.Contains(stem) || (l.Topics != null && l.Topics.Contains(stem)) || (l.Description != null && l.Description.Contains(stem)))));
                }
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
                        Topics = l.Topics,
                        DetailsUrl = $"/Resource/LectureDetails/{l.Id}",
                        DownloadUrl = $"/Resource/Download/{l.Id}?type=lecture",
                        DownloadCount = l.DownloadCount,
                        ViewCount = l.ViewCount
                    });
                }

                var sb = new StringBuilder();
                sb.Append($"<p>I found <strong>{lectures.Count} lecture material{(lectures.Count > 1 ? "s" : "")}</strong> for <strong>{targetLabel}</strong> in the database:</p>");
                sb.Append("<ul style=\"margin: 0.5rem 0 0.75rem 1.25rem; padding: 0;\">");
                foreach (var l in lectures)
                {
                    var topicsInfo = string.IsNullOrWhiteSpace(l.Topics) ? "" : $" • Topics: {l.Topics}";
                    sb.Append("<li style=\"margin-bottom: 0.6rem;\">");
                    sb.Append($"🎥 <strong><a href=\"/Resource/LectureDetails/{l.Id}\">{l.Title}</a></strong> ");
                    sb.Append($"<span style=\"color: #9ca3af;\">({l.CourseCode} • {l.Semester} {(int)l.Year}{topicsInfo})</span><br/>");
                    sb.Append($"<small style=\"color: #6b7280;\">Subject: {l.SubjectName} | Downloads: {l.DownloadCount}</small><br/>");
                    sb.Append($"<a href=\"/Resource/LectureDetails/{l.Id}\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">View Details &rarr;</a> &nbsp;|&nbsp; ");
                    sb.Append($"<a href=\"/Resource/Download/{l.Id}?type=lecture\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">Download Slides &darr;</a>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append($"<p style=\"margin-top: 0.5rem;\">You can browse other lectures and video slides on the <a href=\"/Resource/Lectures?search={Uri.EscapeDataString(intent.CanonicalCourseCode ?? intent.TopicOrSubject ?? string.Empty)}\">Lectures catalog</a>.</p>");
                response.Reply = sb.ToString();
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append($"<p>I searched the database through <code>ApplicationDbContext</code> for <strong>{targetLabel}</strong> lecture materials, but no approved lectures were found.</p>");

                // Check if notes exist for this subject/course
                var relatedNotes = await _context.Notes
                    .Where(n => n.Status == ResourceStatus.Approved && !string.IsNullOrEmpty(intent.CanonicalCourseCode) && (
                        n.CourseCode.Contains(intent.CanonicalCourseCode) ||
                        n.SubjectName.Contains(intent.CanonicalCourseCode)))
                    .Take(3)
                    .ToListAsync();

                if (relatedNotes.Count > 0)
                {
                    sb.Append($"<p>However, study notes are available for <strong>{intent.CanonicalCourseCode}</strong>:</p><ul>");
                    foreach (var n in relatedNotes)
                    {
                        sb.Append($"<li>📝 <a href=\"/Resource/NoteDetails/{n.Id}\"><strong>{n.Title}</strong></a> ({n.SubjectName})</li>");
                    }
                    sb.Append("</ul>");
                }

                sb.Append($"<p>Explore all available slides and recordings on the <a href=\"/Resource/Lectures\">Lectures Page</a>.</p>");
                response.Reply = sb.ToString();
            }
        }

        private async Task SearchAllResourcesAsync(AiQueryResponse response, QueryIntent intent)
        {
            var targetLabel = intent.GetDisplayTarget();

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

            // Apply Course Code filter
            if (!string.IsNullOrEmpty(intent.CanonicalCourseCode))
            {
                var canonical = intent.CanonicalCourseCode;
                var compact = intent.CompactCourseCode;
                var dept = intent.DeptLetters;
                var num = intent.CourseNumber;

                papersQuery = papersQuery.Where(p =>
                    p.CourseCode.Contains(canonical) ||
                    (!string.IsNullOrEmpty(compact) && p.CourseCode.Contains(compact)) ||
                    (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(num) && p.CourseCode.Contains(dept) && p.CourseCode.Contains(num)) ||
                    p.Title.Contains(canonical) ||
                    p.SubjectName.Contains(canonical));

                notesQuery = notesQuery.Where(n =>
                    n.CourseCode.Contains(canonical) ||
                    (!string.IsNullOrEmpty(compact) && n.CourseCode.Contains(compact)) ||
                    (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(num) && n.CourseCode.Contains(dept) && n.CourseCode.Contains(num)) ||
                    n.Title.Contains(canonical) ||
                    n.SubjectName.Contains(canonical));

                lecturesQuery = lecturesQuery.Where(l =>
                    l.CourseCode.Contains(canonical) ||
                    (!string.IsNullOrEmpty(compact) && l.CourseCode.Contains(compact)) ||
                    (!string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(num) && l.CourseCode.Contains(dept) && l.CourseCode.Contains(num)) ||
                    l.Title.Contains(canonical) ||
                    l.SubjectName.Contains(canonical));
            }

            // Apply Year filter
            if (intent.Year.HasValue)
            {
                papersQuery = papersQuery.Where(p => (int)p.Year == intent.Year.Value);
                notesQuery = notesQuery.Where(n => (int)n.Year == intent.Year.Value);
                lecturesQuery = lecturesQuery.Where(l => (int)l.Year == intent.Year.Value);
            }

            // Apply Semester filter
            if (intent.Semester.HasValue)
            {
                papersQuery = papersQuery.Where(p => p.Semester == intent.Semester.Value);
                notesQuery = notesQuery.Where(n => n.Semester == intent.Semester.Value);
                lecturesQuery = lecturesQuery.Where(l => l.Semester == intent.Semester.Value);
            }

            // Apply Topic / Subject filter
            if (!string.IsNullOrEmpty(intent.TopicOrSubject))
            {
                var topic = intent.TopicOrSubject;
                var stem = intent.TopicStem;

                if (intent.TopicWords.Count >= 2)
                {
                    var w0 = intent.TopicWords[0];
                    var w1 = intent.TopicWords[1];

                    papersQuery = papersQuery.Where(p =>
                        p.SubjectName.Contains(topic) ||
                        p.Title.Contains(topic) ||
                        p.CourseCode.Contains(topic) ||
                        (p.Description != null && p.Description.Contains(topic)) ||
                        p.ProfessorName.Contains(topic) ||
                        ((p.SubjectName.Contains(w0) || p.Title.Contains(w0)) && (p.SubjectName.Contains(w1) || p.Title.Contains(w1))));

                    notesQuery = notesQuery.Where(n =>
                        n.SubjectName.Contains(topic) ||
                        n.Title.Contains(topic) ||
                        n.CourseCode.Contains(topic) ||
                        (n.Description != null && n.Description.Contains(topic)) ||
                        (n.ProfessorName != null && n.ProfessorName.Contains(topic)) ||
                        ((n.SubjectName.Contains(w0) || n.Title.Contains(w0)) && (n.SubjectName.Contains(w1) || n.Title.Contains(w1))));

                    lecturesQuery = lecturesQuery.Where(l =>
                        l.SubjectName.Contains(topic) ||
                        l.Title.Contains(topic) ||
                        l.CourseCode.Contains(topic) ||
                        (l.Topics != null && l.Topics.Contains(topic)) ||
                        (l.Description != null && l.Description.Contains(topic)) ||
                        (l.ProfessorName != null && l.ProfessorName.Contains(topic)) ||
                        ((l.SubjectName.Contains(w0) || l.Title.Contains(w0)) && (l.SubjectName.Contains(w1) || l.Title.Contains(w1))));
                }
                else
                {
                    papersQuery = papersQuery.Where(p =>
                        p.SubjectName.Contains(topic) ||
                        p.Title.Contains(topic) ||
                        p.CourseCode.Contains(topic) ||
                        (p.Description != null && p.Description.Contains(topic)) ||
                        p.ProfessorName.Contains(topic) ||
                        (!string.IsNullOrEmpty(stem) && (p.SubjectName.Contains(stem) || p.Title.Contains(stem) || (p.Description != null && p.Description.Contains(stem)))));

                    notesQuery = notesQuery.Where(n =>
                        n.SubjectName.Contains(topic) ||
                        n.Title.Contains(topic) ||
                        n.CourseCode.Contains(topic) ||
                        (n.Description != null && n.Description.Contains(topic)) ||
                        (n.ProfessorName != null && n.ProfessorName.Contains(topic)) ||
                        (!string.IsNullOrEmpty(stem) && (n.SubjectName.Contains(stem) || n.Title.Contains(stem) || (n.Description != null && n.Description.Contains(stem)))));

                    lecturesQuery = lecturesQuery.Where(l =>
                        l.SubjectName.Contains(topic) ||
                        l.Title.Contains(topic) ||
                        l.CourseCode.Contains(topic) ||
                        (l.Topics != null && l.Topics.Contains(topic)) ||
                        (l.Description != null && l.Description.Contains(topic)) ||
                        (l.ProfessorName != null && l.ProfessorName.Contains(topic)) ||
                        (!string.IsNullOrEmpty(stem) && (l.SubjectName.Contains(stem) || l.Title.Contains(stem) || (l.Topics != null && l.Topics.Contains(stem)) || (l.Description != null && l.Description.Contains(stem)))));
                }
            }

            var papers = await papersQuery.OrderByDescending(p => p.Year).Take(6).ToListAsync();
            var notes = await notesQuery.OrderByDescending(n => n.AverageRating).Take(6).ToListAsync();
            var lectures = await lecturesQuery.OrderByDescending(l => l.CreatedDate).Take(6).ToListAsync();

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
                    Topics = l.Topics,
                    DetailsUrl = $"/Resource/LectureDetails/{l.Id}",
                    DownloadUrl = $"/Resource/Download/{l.Id}?type=lecture",
                    DownloadCount = l.DownloadCount,
                    ViewCount = l.ViewCount
                });
            }

            if (total > 0)
            {
                var sb = new StringBuilder();
                sb.Append($"<p>I found <strong>{total} resource{(total > 1 ? "s" : "")}</strong> matching <strong>{targetLabel}</strong> across all course materials in the database:</p>");
                sb.Append("<ul style=\"margin: 0.5rem 0 0.75rem 1.25rem; padding: 0;\">");

                foreach (var p in papers)
                {
                    sb.Append("<li style=\"margin-bottom: 0.6rem;\">");
                    sb.Append($"📄 <strong>Past Paper:</strong> <a href=\"/Resource/PastPaperDetails/{p.Id}\">{p.Title}</a> ");
                    sb.Append($"<span style=\"color: #9ca3af;\">({p.CourseCode} • {p.Semester} {(int)p.Year})</span><br/>");
                    sb.Append($"<a href=\"/Resource/PastPaperDetails/{p.Id}\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">View Details &rarr;</a> &nbsp;|&nbsp; ");
                    sb.Append($"<a href=\"/Resource/Download/{p.Id}?type=paper\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">Download File &darr;</a>");
                    sb.Append("</li>");
                }

                foreach (var n in notes)
                {
                    var ratingStr = n.TotalRatings > 0 ? $" • ★ {n.AverageRating:F1}" : "";
                    sb.Append("<li style=\"margin-bottom: 0.6rem;\">");
                    sb.Append($"📝 <strong>Study Note:</strong> <a href=\"/Resource/NoteDetails/{n.Id}\">{n.Title}</a> ");
                    sb.Append($"<span style=\"color: #9ca3af;\">({n.CourseCode}{ratingStr})</span><br/>");
                    sb.Append($"<a href=\"/Resource/NoteDetails/{n.Id}\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">View Details &rarr;</a> &nbsp;|&nbsp; ");
                    sb.Append($"<a href=\"/Resource/Download/{n.Id}?type=note\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">Download Note &darr;</a>");
                    sb.Append("</li>");
                }

                foreach (var l in lectures)
                {
                    var topicsInfo = string.IsNullOrWhiteSpace(l.Topics) ? "" : $" • {l.Topics}";
                    sb.Append("<li style=\"margin-bottom: 0.6rem;\">");
                    sb.Append($"🎥 <strong>Lecture:</strong> <a href=\"/Resource/LectureDetails/{l.Id}\">{l.Title}</a> ");
                    sb.Append($"<span style=\"color: #9ca3af;\">({l.CourseCode}{topicsInfo})</span><br/>");
                    sb.Append($"<a href=\"/Resource/LectureDetails/{l.Id}\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">View Details &rarr;</a> &nbsp;|&nbsp; ");
                    sb.Append($"<a href=\"/Resource/Download/{l.Id}?type=lecture\" style=\"font-size: 0.8rem; font-weight: 600; text-decoration: underline;\">Download Slides &darr;</a>");
                    sb.Append("</li>");
                }

                sb.Append("</ul>");
                sb.Append($"<p style=\"margin-top: 0.5rem;\">You can also explore all items by section: <a href=\"/Resource/PastPapers\">Past Papers</a> | <a href=\"/Resource/Notes\">Notes</a> | <a href=\"/Resource/Lectures\">Lectures</a></p>");
                response.Reply = sb.ToString();
            }
            else
            {
                response.Reply = $"<p>I searched the database through <code>ApplicationDbContext</code> for <strong>{targetLabel}</strong>, but no matching approved resources were found. You can browse all materials in the <a href=\"/Resource/PastPapers\">Past Papers</a>, <a href=\"/Resource/Notes\">Notes</a>, <a href=\"/Resource/Lectures\">Lectures</a>, or <a href=\"/StarterKit\">Starter Kit</a> sections.</p>";
            }
        }

        #endregion

        #region Guidance Helpers

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

        #endregion
    }
}
