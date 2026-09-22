using System.ComponentModel.DataAnnotations;

namespace student_resource_hub.Models
{
    public class ResourceModerationRequest
    {
        public int Id { get; set; }
        public int RequestedByUserId { get; set; }
        [Required, StringLength(20)] public string ResourceType { get; set; } = string.Empty;
        public int ResourceId { get; set; }
        [Required, StringLength(20)] public string Action { get; set; } = string.Empty;
        [Required, StringLength(20)] public string Status { get; set; } = "Pending";
        [StringLength(500)] public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
        public int? ReviewedByUserId { get; set; }
        public User? RequestedByUser { get; set; }
    }
}
