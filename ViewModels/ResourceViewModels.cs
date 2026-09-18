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

        [StringLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Professor name is required.")]
        [StringLength(200)]
        public string ProfessorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a department.")]
        public Department Department { get; set; }

        public int? AcademicDepartmentId { get; set; }

        public int? AcademicSemesterId { get; set; }

        public int? AcademicCourseId { get; set; }
        public int? AcademicCatalogSemesterId { get; set; }

        public List<AcademicDepartmentOptionViewModel> Departments { get; set; } = new();
        public List<AcademicSemesterOptionViewModel> Semesters { get; set; } = new();
        public List<AcademicCourseOptionViewModel> Courses { get; set; } = new();

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

        [StringLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(200)]
        public string? ProfessorName { get; set; }

        [Required(ErrorMessage = "Please select a department.")]
        public Department Department { get; set; }

        public int? AcademicDepartmentId { get; set; }

        public int? AcademicSemesterId { get; set; }

        public int? AcademicCourseId { get; set; }
        public int? AcademicCatalogSemesterId { get; set; }

        public List<AcademicDepartmentOptionViewModel> Departments { get; set; } = new();
        public List<AcademicSemesterOptionViewModel> Semesters { get; set; } = new();
        public List<AcademicCourseOptionViewModel> Courses { get; set; } = new();

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

        public int? AcademicDepartmentId { get; set; }

        public int? AcademicSemesterId { get; set; }

        public int? AcademicCourseId { get; set; }
        public int? AcademicCatalogSemesterId { get; set; }

        public List<AcademicDepartmentOptionViewModel> Departments { get; set; } = new();
        public List<AcademicSemesterOptionViewModel> Semesters { get; set; } = new();
        public List<AcademicCourseOptionViewModel> Courses { get; set; } = new();

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
        public List<ResourceCourseGroup<T>> CourseGroups { get; set; } = new();
        public List<CourseCatalogGroup> CourseCatalogGroups { get; set; } = new();
        public List<ResourceBrowseGroup> DepartmentGroups { get; set; } = new();
        public List<ResourceBrowseGroup> SemesterGroups { get; set; } = new();
        public List<ResourceTermOption> ResourceTerms { get; set; } = new();
        public int? SelectedAcademicDepartmentId { get; set; }
        public int? SelectedAcademicSemesterId { get; set; }
        public int? SelectedAcademicTermId { get; set; }
        public string? SelectedCourseCode { get; set; }
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

    public class ResourceBrowseGroup
    {
        public int AcademicDepartmentId { get; set; }
        public int? AcademicSemesterId { get; set; }
        public Department Department { get; set; }
        public Semester? Semester { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? UniversityName { get; set; }
        public int Count { get; set; }
    }

    public class ResourceCourseGroup<T> where T : class
    {
        public string CourseCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public List<T> Resources { get; set; } = new();
    }

    public class CourseCatalogGroup
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsLab { get; set; }
        public int ResourceCount { get; set; }
    }

    public class ResourceTermOption
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AcademicYear { get; set; }
        public string TermName { get; set; } = string.Empty;
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
