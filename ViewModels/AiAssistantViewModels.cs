using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.ViewModels
{
    public class AiQueryRequest
    {
        [Required]
        public string Query { get; set; } = string.Empty;
    }

    public class AiResourceItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty; // "Past Paper", "Note", "Lecture"
        public string Department { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string ProfessorName { get; set; } = string.Empty;
        public string DetailsUrl { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
        public int DownloadCount { get; set; }
        public int ViewCount { get; set; }
        public decimal? AverageRating { get; set; }
        public string? Topics { get; set; }
    }

    public class AiQueryResponse
    {
        public bool Success { get; set; } = true;
        public string Query { get; set; } = string.Empty;
        public string Reply { get; set; } = string.Empty;
        public int TotalMatches { get; set; }
        public List<AiResourceItemDto> Resources { get; set; } = new();
    }
}
