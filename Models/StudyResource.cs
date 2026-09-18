using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class StudyResource
    {
        public int Id { get; set; }
        public int StudySessionId { get; set; }
        public int? StudyFolderId { get; set; }

        [Required, StringLength(20)]
        public string ResourceType { get; set; } = string.Empty;

        public int ResourceId { get; set; }

        [Required, StringLength(200)]
        public string ResourceTitle { get; set; } = string.Empty;

        [StringLength(50)]
        public string? CourseCode { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public StudySession? StudySession { get; set; }
        public StudyFolder? StudyFolder { get; set; }
    }
}