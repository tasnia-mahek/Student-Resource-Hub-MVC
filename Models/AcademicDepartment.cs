using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class AcademicDepartment
    {
        public int Id { get; set; }
        public int UniversityId { get; set; }

        [Required, StringLength(160)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public University? University { get; set; }
        public ICollection<AcademicSemester> Semesters { get; set; } = new List<AcademicSemester>();
    }
}