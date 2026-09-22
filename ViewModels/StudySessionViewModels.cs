using System.ComponentModel.DataAnnotations;
using student_resource_hub.Models;

namespace student_resource_hub.ViewModels
{
    public class StudySessionsViewModel
    {
        public List<StudySessionSummaryViewModel> Sessions { get; set; } = new();
        public StudySessionFormViewModel NewSession { get; set; } = new();
    }

    public class StudySessionSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int FolderCount { get; set; }
        public int ResourceCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class StudySessionDetailsViewModel
    {
        public StudySession Session { get; set; } = new();
        public StudyFolderFormViewModel NewFolder { get; set; } = new();
        public string? ResourceType { get; set; }
        public int? ResourceId { get; set; }
        public string? ResourceTitle { get; set; }
        public string? CourseCode { get; set; }
        public int? SelectedFolderId { get; set; }
        public int? CurrentFolderId { get; set; }
        public string? CurrentFolderName { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class StudySessionFormViewModel
    {
        [Required, StringLength(160)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }

    public class StudyFolderFormViewModel
    {
        public int StudySessionId { get; set; }
        public int? ParentFolderId { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = string.Empty;
    }
}