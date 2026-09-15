using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class AttendanceSession
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string SessionTitle { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        public DateTime SessionDate { get; set; } = DateTime.Today;

        [Required]
        public TimeSpan StartTime { get; set; } = new TimeSpan(9, 0, 0);

        [Required]
        public TimeSpan EndTime { get; set; } = new TimeSpan(11, 0, 0);

        [Required]
        [MaxLength(50)]
        public string Passcode { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    }
}
