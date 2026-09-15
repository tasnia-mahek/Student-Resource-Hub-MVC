using System.ComponentModel.DataAnnotations;
using student_resource_hub.Models;

namespace student_resource_hub.ViewModels
{
    public class AttendanceDashboardViewModel
    {
        public bool IsAdmin { get; set; }
        public bool IsCR { get; set; }
        public bool IsStudent { get; set; }
        public string CurrentUserRole { get; set; } = "Student";
        public string CurrentUserName { get; set; } = string.Empty;

        // Stats
        public int TotalAvailableSessions { get; set; }
        public int TotalAttendedSessions { get; set; }
        public double AttendancePercentage => TotalAvailableSessions > 0 
            ? Math.Round((double)TotalAttendedSessions / TotalAvailableSessions * 100, 1) 
            : 100.0;

        // Session Collections
        public List<AttendanceSession> ActiveSessions { get; set; } = new();
        public List<AttendanceSession> AllSessions { get; set; } = new();
        public List<AttendanceRecord> MyAttendanceRecords { get; set; } = new();
        public HashSet<int> AttendedSessionIds { get; set; } = new();

        // Admin creation form bind
        public CreateAttendanceSessionViewModel NewSession { get; set; } = new();

        // Check-in bind
        public CheckInAttendanceViewModel CheckIn { get; set; } = new();
    }

    public class CreateAttendanceSessionViewModel
    {
        [Required(ErrorMessage = "Session title is required (e.g. Midterm Review, Lecture 08)")]
        [StringLength(200)]
        [Display(Name = "Session Title")]
        public string SessionTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course code is required (e.g. CSE-320, EEE-211)")]
        [StringLength(50)]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required")]
        [StringLength(100)]
        [Display(Name = "Department")]
        public string Department { get; set; } = "ComputerScience";

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Session Date")]
        public DateTime SessionDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay;

        [Required]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; } = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(2));

        [Required(ErrorMessage = "A passcode is required for students to check in")]
        [StringLength(50)]
        [Display(Name = "Session Passcode")]
        public string Passcode { get; set; } = string.Empty;
    }

    public class CheckInAttendanceViewModel
    {
        [Required(ErrorMessage = "Please select a session")]
        public int SessionId { get; set; }

        [Required(ErrorMessage = "Please enter the session passcode")]
        [StringLength(50)]
        public string Passcode { get; set; } = string.Empty;
    }

    public class SessionRosterViewModel
    {
        public AttendanceSession Session { get; set; } = null!;
        public List<AttendanceRecord> Attendees { get; set; } = new();
        public bool CanManage { get; set; }
    }
}
