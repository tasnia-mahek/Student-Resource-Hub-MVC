using System;
using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }

        public int AttendanceSessionId { get; set; }
        public AttendanceSession? AttendanceSession { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [MaxLength(120)]
        public string StudentName { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string StudentEmail { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "Student";

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Present";
    }
}
