using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a role.")]
        [RegularExpression("^(Student|Admin|CR)$", ErrorMessage = "Please select a valid role.")]
        public string Role { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}
