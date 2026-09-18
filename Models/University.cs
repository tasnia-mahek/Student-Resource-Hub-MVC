using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class University
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? LogoUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<AcademicDepartment> Departments { get; set; } = new List<AcademicDepartment>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}