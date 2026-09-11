using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace student_resource_hub.Models
{
    public class PastPaper
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject name is required.")]
        [StringLength(200)]
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course code is required.")]
        [StringLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(200)]
        public string ProfessorName { get; set; } = string.Empty;

        [Required]
        public Department Department { get; set; }

        [Required]
        public Year Year { get; set; }

        [Required]
        public Semester Semester { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? FileType { get; set; }

        public long FileSizeBytes { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UploadedByUserId { get; set; }

        public virtual User? UploadedByUser { get; set; }

        [Required]
        public ResourceStatus Status { get; set; } = ResourceStatus.Pending;

        public int DownloadCount { get; set; } = 0;

        public int ViewCount { get; set; } = 0;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [StringLength(500)]
        public string? ApprovalNotes { get; set; }
    }
}
