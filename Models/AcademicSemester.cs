using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class AcademicSemester
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }

        public int? AcademicYear { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [StringLength(30)]
        public string? TermName { get; set; }

        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public AcademicDepartment? Department { get; set; }
        public ICollection<AcademicCourse> Courses { get; set; } = new List<AcademicCourse>();
    }
}