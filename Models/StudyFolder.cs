using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class StudyFolder
    {
        public int Id { get; set; }
        public int StudySessionId { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = string.Empty;

        public StudySession? StudySession { get; set; }
        public ICollection<StudyResource> Resources { get; set; } = new List<StudyResource>();
    }
}