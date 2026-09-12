using System.ComponentModel.DataAnnotations;
using student_resource_hub.Models;

namespace student_resource_hub.ViewModels
{
    // Upload Past Paper ViewModel
    public class UploadResourceViewModel
    {
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

        [Required(ErrorMessage = "Professor name is required.")]
        [StringLength(200)]
        public string ProfessorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a department.")]
        public Department Department { get; set; }

        [Required(ErrorMessage = "Please select a year.")]
        public Year Year { get; set; }

        [Required(ErrorMessage = "Please select a semester.")]
        public Semester Semester { get; set; }

        [Required(ErrorMessage = "Please select a file to upload.")]
        public IFormFile? File { get; set; }
    }

    // Upload Note ViewModel
    public class UploadNoteViewModel
    {
        [Required(ErrorMessage = "Note title is required.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject name is required.")]
        [StringLength(200)]
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course code is required.")]
        [StringLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(200)]
        public string? ProfessorName { get; set; }

        [Required(ErrorMessage = "Please select a department.")]
        public Department Department { get; set; }

        [Required(ErrorMessage = "Please select a year.")]
        public Year Year { get; set; }

        [Required(ErrorMessage = "Please select a semester.")]
        public Semester Semester { get; set; }

        [Required(ErrorMessage = "Please select a file to upload.")]
        public IFormFile? File { get; set; }
    }

    // Upload Lecture ViewModel
    public class UploadLectureViewModel
    {
        [Required(ErrorMessage = "Lecture title is required.")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject name is required.")]
        [StringLength(200)]
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course code is required.")]
        [StringLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(200)]
        public string? ProfessorName { get; set; }

        [StringLength(100)]
        public string? Topics { get; set; }

        [Required(ErrorMessage = "Please select a department.")]
        public Department Department { get; set; }

        [Required(ErrorMessage = "Please select a year.")]
        public Year Year { get; set; }

        [Required(ErrorMessage = "Please select a semester.")]
        public Semester Semester { get; set; }

        [Required(ErrorMessage = "Please select a file to upload.")]
        public IFormFile? File { get; set; }
    }

    // Resource List ViewModel (Generic for both types)
    public class ResourceListViewModel<T> where T : class
    {
        public List<T> Resources { get; set; } = new();
        public string? SelectedDepartment { get; set; }
        public int? SelectedYear { get; set; }
        public string? SelectedSemester { get; set; }
        public string SelectedSort { get; set; } = "newest";
        public string? SearchTerm { get; set; }

        // Helper lists for dropdown filters
        public List<string> Departments => GetDepartmentList();
        public List<int> Years => GetYearList();
        public List<string> Semesters => GetSemesterList();

        private List<string> GetDepartmentList()
        {
            return Enum.GetNames(typeof(Department)).ToList();
        }

        private List<int> GetYearList()
        {
            return Enum.GetValues(typeof(Year))
                .Cast<Year>()
                .Select(y => (int)y)
                .OrderByDescending(y => y)
                .ToList();
        }

        private List<string> GetSemesterList()
        {
            return Enum.GetNames(typeof(Semester)).ToList();
        }
    }

    // Filter ViewModel for easy filtering
    public class ResourceFilterViewModel
    {
        public string? Department { get; set; }
        public int? Year { get; set; }
        public string? Semester { get; set; }
        public string SortBy { get; set; } = "newest";
    }
}
