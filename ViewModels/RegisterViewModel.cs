using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a password.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a role.")]
        [RegularExpression("^(Student|Admin|CR)$", ErrorMessage = "Please select a valid role.")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a university.")]
        public int? UniversityId { get; set; }

        [Required(ErrorMessage = "Please select a department.")]
        public int? AcademicDepartmentId { get; set; }

        [Required(ErrorMessage = "Please select a semester.")]
        public int? AcademicSemesterId { get; set; }

        public List<UniversityOptionViewModel> Universities { get; set; } = new();
        public List<AcademicDepartmentOptionViewModel> Departments { get; set; } = new();
        public List<AcademicSemesterOptionViewModel> Semesters { get; set; } = new();
    }

    public class UniversityOptionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AcademicDepartmentOptionViewModel
    {
        public int Id { get; set; }
        public int UniversityId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AcademicSemesterOptionViewModel
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AcademicCourseOptionViewModel
    {
        public int Id { get; set; }
        public int AcademicSemesterId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
