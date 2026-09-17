using student_resource_hub.Models;

namespace student_resource_hub.ViewModels
{
    public class StarterKitViewModel
    {
        public string? SelectedDepartment { get; set; }
        public string? SelectedSemester { get; set; }
        public bool IsGenerated { get; set; } = false;

        public List<DepartmentOption> DepartmentOptions => new()
        {
            new DepartmentOption { Value = "ComputerScience", DisplayName = "Computer Science", Icon = "💻", Code = "CSE" },
            new DepartmentOption { Value = "Engineering", DisplayName = "Engineering", Icon = "⚙️", Code = "ENG" },
            new DepartmentOption { Value = "Mathematics", DisplayName = "Mathematics", Icon = "📐", Code = "MATH" },
            new DepartmentOption { Value = "Physics", DisplayName = "Physics", Icon = "⚡", Code = "PHY" },
            new DepartmentOption { Value = "Chemistry", DisplayName = "Chemistry", Icon = "🧪", Code = "CHEM" },
            new DepartmentOption { Value = "Business", DisplayName = "Business & Admin", Icon = "📊", Code = "BBA" }
        };

        public List<SemesterOption> SemesterOptions => new()
        {
            new SemesterOption { Value = "Fall", DisplayName = "Fall Semester", Description = "Autumn / Term 1 intake", Badge = "Term 1" },
            new SemesterOption { Value = "Spring", DisplayName = "Spring Semester", Description = "Spring / Term 2 intake", Badge = "Term 2" },
            new SemesterOption { Value = "Summer", DisplayName = "Summer Term", Description = "Mid-year / Accelerated term", Badge = "Term 3" }
        };

        public List<StarterKitSubjectGroupViewModel> SubjectGroups { get; set; } = new();

        public int TotalResourcesCount => SubjectGroups.Sum(s => s.PastPapers.Count + s.Notes.Count + s.Lectures.Count);
        public int TotalSubjectsCount => SubjectGroups.Count;

        public string GetDepartmentDisplayName()
        {
            var match = DepartmentOptions.FirstOrDefault(d => string.Equals(d.Value, SelectedDepartment, StringComparison.OrdinalIgnoreCase));
            return match?.DisplayName ?? SelectedDepartment ?? "Department";
        }

        public string GetSemesterDisplayName()
        {
            var match = SemesterOptions.FirstOrDefault(s => string.Equals(s.Value, SelectedSemester, StringComparison.OrdinalIgnoreCase));
            return match?.DisplayName ?? SelectedSemester ?? "Semester";
        }
    }

    public class DepartmentOption
    {
        public string Value { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class SemesterOption
    {
        public string Value { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Badge { get; set; } = string.Empty;
    }

    public class StarterKitSubjectGroupViewModel
    {
        public string CourseCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;

        public List<PastPaper> PastPapers { get; set; } = new();
        public List<Note> Notes { get; set; } = new();
        public List<Lecture> Lectures { get; set; } = new();

        public int PastPapersCount => PastPapers.Count;
        public int NotesCount => Notes.Count;
        public int LecturesCount => Lectures.Count;

        public bool HasPastPapers => PastPapers.Count > 0;
        public bool HasNotes => Notes.Count > 0;
        public bool HasLectures => Lectures.Count > 0;

        public int TotalCount => PastPapersCount + NotesCount + LecturesCount;
    }
}
