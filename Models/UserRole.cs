using System;

namespace AuthApi.Models
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
