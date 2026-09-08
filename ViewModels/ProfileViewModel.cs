using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace student_resource_hub.ViewModels
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? ProfilePhotoPath { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOwner { get; set; }

        [Required, StringLength(120)]
        public string EditedFullName { get; set; } = string.Empty;

        [StringLength(500)]
        public string EditedBio { get; set; } = string.Empty;

        [StringLength(120)]
        public string EditedLocation { get; set; } = string.Empty;

        [Phone, StringLength(30)]
        public string EditedPhoneNumber { get; set; } = string.Empty;

        public IFormFile? ProfilePhoto { get; set; }
    }
}