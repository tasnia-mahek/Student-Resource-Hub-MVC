using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using student_resource_hub.Data;
using student_resource_hub.Models;

namespace student_resource_hub.Controllers
{
    [Authorize]
    public class AiAssistantController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AiAssistantController> _logger;

        public AiAssistantController(ApplicationDbContext context, ILogger<AiAssistantController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] AiChatRequest request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { error = "Please enter a question first." });
            }

            var reply = await BuildReplyAsync(request.Message.Trim());
            return Json(new { reply });
        }

        private async Task<string> BuildReplyAsync(string message)
        {
            var normalized = message.Trim();
            var lowered = normalized.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(normalized))
            {
                return "Please ask a question about your studies, courses, or resources.";
            }

            var courseCodes = ExtractCourseCodes(normalized).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            if (lowered.Contains("past paper") || lowered.Contains("past papers") || lowered.Contains("exam paper") || lowered.Contains("question paper"))
            {
                var papers = await _context.PastPapers
                    .AsNoTracking()
                    .Where(item => item.Status == ResourceStatus.Approved)
                    .OrderByDescending(item => item.CreatedDate)
                    .Take(3)
                    .Select(item => new { item.CourseCode, item.Title })
                    .ToListAsync();

                if (papers.Count > 0)
                {
                    var summary = string.Join("; ", papers.Select(item => $"{item.CourseCode} - {item.Title}"));
                    return $"I found recent approved past papers in the system: {summary}. You can browse them from the resource library and filter by course code.";
                }

                return "I could not find any approved past papers yet. Try checking the resource library or ask for a specific course code.";
            }

            if (lowered.Contains("note") || lowered.Contains("notes") || lowered.Contains("study note"))
            {
                var notes = await _context.Notes
                    .AsNoTracking()
                    .Where(item => item.Status == ResourceStatus.Approved)
                    .OrderByDescending(item => item.CreatedDate)
                    .Take(3)
                    .Select(item => new { item.CourseCode, item.Title })
                    .ToListAsync();

                if (notes.Count > 0)
                {
                    var summary = string.Join("; ", notes.Select(item => $"{item.CourseCode} - {item.Title}"));
                    return $"I found these approved study notes: {summary}. They are a strong starting point for quick revision and topic review.";
                }

                return "I did not find any approved study notes for the moment. You can upload a note or search by course code.";
            }

            if (lowered.Contains("lecture") || lowered.Contains("video") || lowered.Contains("tutorial"))
            {
                var lectures = await _context.Lectures
                    .AsNoTracking()
                    .Where(item => item.Status == ResourceStatus.Approved)
                    .OrderByDescending(item => item.CreatedDate)
                    .Take(3)
                    .Select(item => new { item.CourseCode, item.Title })
                    .ToListAsync();

                if (lectures.Count > 0)
                {
                    var summary = string.Join("; ", lectures.Select(item => $"{item.CourseCode} - {item.Title}"));
                    return $"Here are some available lectures: {summary}. These are useful for revision, concept review, and exam preparation.";
                }

                return "I did not find any approved lecture videos yet. Try checking the lecture library or ask for a specific course code.";
            }

            if (courseCodes.Count > 0)
            {
                var formattedCodes = string.Join(", ", courseCodes);

                var matches = await _context.Notes
                    .AsNoTracking()
                    .Where(item => item.Status == ResourceStatus.Approved && courseCodes.Contains(item.CourseCode))
                    .Select(item => new { item.CourseCode, item.Title })
                    .Take(3)
                    .ToListAsync();

                var papers = await _context.PastPapers
                    .AsNoTracking()
                    .Where(item => item.Status == ResourceStatus.Approved && courseCodes.Contains(item.CourseCode))
                    .Select(item => new { item.CourseCode, item.Title })
                    .Take(3)
                    .ToListAsync();

                var lectures = await _context.Lectures
                    .AsNoTracking()
                    .Where(item => item.Status == ResourceStatus.Approved && courseCodes.Contains(item.CourseCode))
                    .Select(item => new { item.CourseCode, item.Title })
                    .Take(3)
                    .ToListAsync();

                var parts = new List<string>();
                if (matches.Count > 0) parts.Add("notes: " + string.Join(", ", matches.Select(item => item.Title)));
                if (papers.Count > 0) parts.Add("past papers: " + string.Join(", ", papers.Select(item => item.Title)));
                if (lectures.Count > 0) parts.Add("lectures: " + string.Join(", ", lectures.Select(item => item.Title)));

                if (parts.Count > 0)
                {
                    return $"For {formattedCodes}, I found relevant resources including {string.Join("; ", parts)}. Use those as your primary revision sources and focus on the most recent files.";
                }
            }

            if (lowered.Contains("starter kit") || lowered.Contains("first year") || lowered.Contains("first-year") || lowered.Contains("freshman"))
            {
                return "The First-Year Starter Kit helps you find core foundational materials quickly. Start with your department, year, and semester filters, then review the approved notes, lectures, and past papers for each course.";
            }

            if (lowered.Contains("study tip") || lowered.Contains("study tips") || lowered.Contains("exam") || lowered.Contains("midterm") || lowered.Contains("final"))
            {
                return "A strong study plan is: review the lecture, summarize key concepts in your own words, solve recent past papers, and revisit weak topics. Focus on active recall and spaced repetition rather than rereading alone.";
            }

            if (lowered.Contains("hello") || lowered.Contains("hi") || lowered.Contains("hey"))
            {
                return "Hello! I can help you find study notes, lecture videos, and past papers for your courses. Ask me for a course code or a specific resource type.";
            }

            return "I can help you find approved notes, lecture videos, and past papers in the resource hub. Try asking for a course code such as CSE 110 or a resource type like past papers, notes, or lectures.";
        }

        private static List<string> ExtractCourseCodes(string message)
        {
            var matches = Regex.Matches(message, @"[A-Za-z]{2,10}\s*\d{2,6}");
            return matches
                .Select(match => match.Value.Trim())
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value.Replace(" ", string.Empty).ToUpperInvariant())
                .ToList();
        }
    }

    public class AiChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }
}
