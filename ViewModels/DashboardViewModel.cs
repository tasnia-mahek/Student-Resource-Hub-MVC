using student_resource_hub.Models;

namespace student_resource_hub.ViewModels
{
    public class DashboardViewModel
    {
        public string UserName { get; set; } = "Scholar";
        public string UserEmail { get; set; } = string.Empty;
        public string UserRole { get; set; } = "Student";
        public string? DepartmentName { get; set; }
        public string ProfilePhotoPath { get; set; } = string.Empty;

        // Metric Counters
        public int TotalPastPapers { get; set; }
        public int TotalNotes { get; set; }
        public int TotalLectures { get; set; }
        public int TotalFavorites { get; set; }
        public int TotalUsers { get; set; }
        public int TotalUniversities { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalCourses { get; set; }
        public int PendingResources { get; set; }
        public int TotalDownloads { get; set; }

        // Attendance stats
        public int TotalAvailableSessions { get; set; }
        public int TotalAttendedSessions { get; set; }
        public double AttendancePercentage => TotalAvailableSessions > 0
            ? Math.Round((double)TotalAttendedSessions / TotalAvailableSessions * 100, 1)
            : 100.0;

        // Live Active Sessions
        public List<AttendanceSession> ActiveAttendanceSessions { get; set; } = new();
        public HashSet<int> AttendedSessionIds { get; set; } = new();

        // Recent items
        public List<PastPaper> RecentPastPapers { get; set; } = new();
        public List<Note> RecentNotes { get; set; } = new();
        public List<Lecture> RecentLectures { get; set; } = new();

        public bool CanUploadPastPapers => UserRole is "Admin" or "CR";
        public bool CanUploadNotes => UserRole is "Admin" or "CR";
        public bool CanUploadLectures => UserRole is "Admin";
    }
}
