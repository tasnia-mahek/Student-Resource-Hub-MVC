using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class StudySession
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required, StringLength(160)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User? User { get; set; }
        public ICollection<StudyFolder> Folders { get; set; } = new List<StudyFolder>();
        public ICollection<StudyResource> Resources { get; set; } = new List<StudyResource>();
    }
}