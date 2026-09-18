using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class AcademicCourse
    {
        public int Id { get; set; }
        public int AcademicSemesterId { get; set; }

        [Required, StringLength(50)]
        public string CourseCode { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public bool IsLab { get; set; }
        public bool IsActive { get; set; } = true;

        public AcademicSemester? AcademicSemester { get; set; }
    }
}