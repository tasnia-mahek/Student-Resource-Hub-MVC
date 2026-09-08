using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Role { get; set; } = string.Empty;

        [StringLength(500)]
        public string Bio { get; set; } = string.Empty;

        [StringLength(120)]
        public string Location { get; set; } = string.Empty;

        [Phone, StringLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(260)]
        public string? ProfilePhotoPath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
