using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.ViewModels
{
    public class AcademicAdminViewModel
    {
        public List<UniversityAdminItemViewModel> Universities { get; set; } = new();
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
    }

    public class SemesterAdminItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
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

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        public int SortOrder { get; set; }
    }
}