using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.ViewModels
{
    public class AcademicAdminViewModel
    {
        public List<UniversityAdminItemViewModel> Universities { get; set; } = new();
        public int? SelectedUniversityId { get; set; }
        public int? SelectedDepartmentId { get; set; }
        public int? SelectedSemesterId { get; set; }
        public UniversityAdminItemViewModel? SelectedUniversity { get; set; }
        public DepartmentAdminItemViewModel? SelectedDepartment { get; set; }
        public SemesterAdminItemViewModel? SelectedSemester { get; set; }
        public UniversityFormViewModel NewUniversity { get; set; } = new();
        public DepartmentFormViewModel NewDepartment { get; set; } = new();
        public SemesterFormViewModel NewSemester { get; set; } = new();
    }

    public class UniversityAdminItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<DepartmentAdminItemViewModel> Departments { get; set; } = new();
    }

    public class DepartmentAdminItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public List<SemesterAdminItemViewModel> Semesters { get; set; } = new();
        public int CourseCount => Semesters.Sum(semester => semester.Courses.Count);
    }

    public class SemesterAdminItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public List<CourseAdminItemViewModel> Courses { get; set; } = new();
    }

    public class CourseAdminItemViewModel
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsLab { get; set; }
    }

    public class UniversityFormViewModel
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }

    public class DepartmentFormViewModel
    {
        [Required]
        public int UniversityId { get; set; }

        [Required, StringLength(160)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageUrl { get; set; }
    }

    public class SemesterFormViewModel
    {
        [Required]
        public int DepartmentId { get; set; }

        [Range(1, 12)]
        public int SemesterNumber { get; set; }

        public string? Name { get; set; }
    }

    public class CourseFormViewModel
    {
        [Required]
        public int SemesterId { get; set; }

        [Required, StringLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public bool IsLab { get; set; }
    }

    public class AdminResourceBrowseViewModel
    {
        public string ResourceType { get; set; } = "paper";
        public int? UniversityId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SemesterId { get; set; }
        public List<AdminResourceUniversityViewModel> Universities { get; set; } = new();
        public AdminResourceUniversityViewModel? University { get; set; }
        public AdminResourceDepartmentViewModel? Department { get; set; }
        public AdminResourceSemesterViewModel? Semester { get; set; }
    }

    public class AdminResourceUniversityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<AdminResourceDepartmentViewModel> Departments { get; set; } = new();
    }

    public class AdminResourceDepartmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public List<AdminResourceSemesterViewModel> Semesters { get; set; } = new();
    }

    public class AdminResourceSemesterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public List<AdminResourceCourseViewModel> Courses { get; set; } = new();
    }

    public class AdminResourceCourseViewModel
    {
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}